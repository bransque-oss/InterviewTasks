using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.Models;

namespace Services
{
    public class CoalWarehouseService
    {
        private const string _commonError = "Something went wrong.";
        private readonly WarehouseContext _context;
        private readonly ILogger<CoalWarehouseService> _logger;

        public CoalWarehouseService(WarehouseContext context, ILogger<CoalWarehouseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Warehouse>> GetWarehouses()
        {
            try
            {
                var warehouses = await _context.Warehouses
                .Include(w => w.Pickets.Where(p => !p.Deleted))
                    .ThenInclude(p => p.Cargoes)
                .Include(w => w.Pickets.Where(p => !p.Deleted))
                    .ThenInclude(p => p.Area)
                .Where(w => !w.Deleted)
                .ToListAsync();

                var areas = await _context.Areas.Include(x => x.Pickets).ToListAsync();

                return warehouses.Select(w => new Warehouse
                {
                    Id = w.Id,
                    Name = w.Name,
                    Pickets = w.Pickets.Select(p => new Picket(p.Id, p.Number, p.AreaId, GetAreaName(p.AreaId, areas))),
                }).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task<IEnumerable<Picket>> GetPickets(int warehouseId)
        {
            await ThrowIfWarehouseIsNotExist(warehouseId);
            try
            {
                var dbPickets = await _context.Pickets
                    .Include(p => p.Area)
                    .Where(x => x.WarehouseId == warehouseId)
                    .ToArrayAsync();

                var areas = await _context.Areas.Include(x => x.Pickets).ToArrayAsync();

                return dbPickets.Select(x => new Picket(x.Id, x.Number, x.AreaId, GetAreaName(x.AreaId, areas)))
                    .OrderBy(x => x.Number);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task<int> AddPicket(PicketInput input)
        {
            await ThrowIfWarehouseIsNotExist(input.WarehouseId);
            await ThrowIfPicketNumberExists(input.Number);

            var picketDal = new PicketDal
            {
                Number = input.Number,
                WarehouseId = input.WarehouseId
            };
            var entry = _context.Pickets.Add(picketDal);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }

            return entry.Entity.Id;
        }
        public async Task UpdatePicket(int picketId, PicketInput input)
        {
            await ThrowIfWarehouseIsNotExist(input.WarehouseId);
            var existingPicket = await GetDbPicket(picketId);
            if (existingPicket.Number != input.Number)
            {
                await ThrowIfPicketNumberExists(input.Number);
            }

            existingPicket.Number = input.Number;
            existingPicket.WarehouseId = input.WarehouseId;
            _context.Pickets.Update(existingPicket);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task DeletePicket(int picketId)
        {
            var existingPicket = await GetDbPicket(picketId);
            existingPicket.Deleted = true;
            existingPicket.Area = null;
            _context.Pickets.Update(existingPicket);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task<IEnumerable<Cargo>> GetCargoes(int warehouseId)
        {
            await ThrowIfWarehouseIsNotExist(warehouseId);
            try
            {
                return await _context.Cargoes
                    .Include(x => x.Picket)
                    .Where(x => x.Picket.WarehouseId == warehouseId)
                    .Select(x => new Cargo(x.Id, x.Weight, x.PicketId, x.Picket.Number))
                    .ToArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task<int> AddCargo(CargoInput input)
        {
            var picket = await GetDbPicket(input.PicketId); 
            var cargo = new CargoDal
            {
                PicketId = picket.Id,
                Weight = input.Weight
            };
            var entry = _context.Cargoes.Add(cargo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }

            return entry.Entity.Id;
        }
        public async Task UpdateCargo(int id, CargoInput input)
        {
            var picket = await GetDbPicket(id);
            var existingCargo = await GetDbCargo(id);
            existingCargo.PicketId = picket.Id;
            existingCargo.Weight = input.Weight;
            _context.Cargoes.Update(existingCargo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task DeleteCargo(int id)
        {
            var cargo = await GetDbCargo(id);
            _context.Cargoes.Remove(cargo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task CombinePicketsToArea(AreaInput input)
        {
            await ThrowIfWarehouseIsNotExist(input.WarehouseId);
            ThrowIfPicketIdsNotExist(input.PicketIds);
            await ThrowIfPicketSequenceNotValid(input);

            try
            {
                var area = new AreaDal();
                _context.Areas.Add(area);
                await _context.SaveChangesAsync();
                await _context.Pickets
                    .Where(p => input.PicketIds.Contains(p.Id))
                    .ExecuteUpdateAsync(e => e.SetProperty(p => p.AreaId, area.Id));

                var history = await CreatePicketAreaHistory(input.WarehouseId);
                _context.PicketAreaHistory.AddRange(history);
                await _context.SaveChangesAsync();

                var emptyAreas = await _context.Areas
                    .Include(a => a.Pickets)
                    .Where(a => !a.Pickets.Any())
                    .ToArrayAsync();
                await _context.Areas
                    .Where(x => emptyAreas.Contains(x))
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(_commonError);
            }
        }
        public async Task<IEnumerable<PicketArea>> GetPicketAreaHistory(int warehouseId, DateTime start, DateTime end)
        {
            var dbHistory = await (from history in _context.PicketAreaHistory
                            join warehouse in _context.Warehouses on history.WarehouseId equals warehouse.Id
                            join picket in _context.Pickets.Include(p => p.Cargoes) on history.PicketId equals picket.Id
                            where history.Created >= start && history.Created <= end && history.WarehouseId == warehouse.Id
                            select new PicketArea
                            {
                                WarehouseName = warehouse.Name,
                                PicketId = history.PicketId,
                                PicketNumber = picket.Number,
                                Weight = picket.Cargoes.Sum(a => a.Weight),
                                AreaName = history.AreaName,
                                Created = history.Created
                            }).ToArrayAsync();

            return dbHistory;
        }

        private async Task ThrowIfWarehouseIsNotExist(int id)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
            if (warehouse is null)
            {
                throw new Exception($"Warehouse Id = '{id}' is not exist.");
            }
        }
        private async Task ThrowIfPicketNumberExists(int newPicketNumber)
        {
            var existingPicket = await _context.Pickets.FirstOrDefaultAsync(x => !x.Deleted && x.Number == newPicketNumber);
            if (existingPicket is not null)
            {
                throw new Exception($"Picket with name '{newPicketNumber}' already exists.");
            }
        }
        private async Task<PicketDal> GetDbPicket(int id)
        {
            var picket = await _context.Pickets.FirstOrDefaultAsync(x => x.Id == id);
            if (picket is null)
            {
                throw new Exception($"Picket with Id = '{id}' doesn't exist.");
            }
            return picket;
        }
        private async Task<CargoDal> GetDbCargo(int id)
        {
            var existingCargo = await _context.Cargoes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCargo == null)
            {
                throw new Exception($"Cargo with Id = '{id}' doesn't exist.");
            }
            return existingCargo;
        }
        private void ThrowIfPicketIdsNotExist(IEnumerable<int> ids)
        {
            var dbPickets = _context.Pickets
                .Where(p => !p.Deleted && ids.Contains(p.Id))
                .ToArray();

            if (dbPickets.Count() == ids.Count())
            {
                return;
            }

            foreach (var id in ids)
            {
                if (dbPickets.All(x => x.Id != id))
                {
                    throw new Exception($"Picket id = '{id}' doesn't exist.");
                }
            }

        }
        private async Task ThrowIfPicketSequenceNotValid(AreaInput input)
        {
            var dbPickets = await _context.Pickets
                .Where(x => x.WarehouseId == input.WarehouseId)
                .OrderBy(x => x.Number)
                .ToListAsync();
            var inputPickets = dbPickets
                .Where(x => input.PicketIds.Contains(x.Id))
                .OrderBy(x => x.Number)
                .ToList();

            var dbPicketGroupSameArea = dbPickets
                .GroupBy(x => x.AreaId)
                .FirstOrDefault(x => x.Key != null && x.Key == inputPickets.First().AreaId);
            if (dbPicketGroupSameArea?.Count() == inputPickets.Count)
            {
                throw new Exception("You try combine pickets which are already combined to same area.");
            }

            var areaStartIndex = dbPickets.FindIndex(x => x.Id == inputPickets.First().Id);
            var areaRange = dbPickets.GetRange(areaStartIndex, inputPickets.Count);
            for (int i = 0; i < inputPickets.Count; i++)
            {
                var idEquals = inputPickets[i].Id == areaRange[i].Id;
                if (!idEquals)
                {
                    throw new Exception("You try combine pickets which aren't located near each other.");
                }
            }
            var startAreaOuterPicketIndex = areaStartIndex - 1;
            var endAreaOuterPicketIndex = areaStartIndex + areaRange.Count;
            if (startAreaOuterPicketIndex >= 0 && endAreaOuterPicketIndex < dbPickets.Count)
            {
                if (dbPickets[startAreaOuterPicketIndex].AreaId == dbPickets[endAreaOuterPicketIndex].AreaId)
                {
                    throw new Exception("You try to insert area into middle of another area.");
                }
            }
        }
        private async Task<IEnumerable<PicketAreaDal>> CreatePicketAreaHistory(int warehouseId)
        {
            var createdTime = DateTime.Now;
            var pickets = await _context.Pickets
                .Include(p => p.Cargoes)
                .Where(p => !p.Deleted && p.WarehouseId == warehouseId)
                .ToArrayAsync();

            var areas = await _context.Areas
                .Include(a => a.Pickets.Where(p => p.WarehouseId == warehouseId))
                .ToArrayAsync();

            return pickets.Select(p => new PicketAreaDal
            {
                WarehouseId = warehouseId,
                PicketId = p.Id,
                AreaName = GetAreaName(p.AreaId, areas),
                Weight = p.Cargoes.Sum(x => x.Weight),
                Created = createdTime
            }).ToArray();
        }
        private string? GetAreaName(int? areaId, IEnumerable<AreaDal> areas)
        {
            if (areaId == null)
            {
                return null;
            }
            var area = areas.FirstOrDefault(x => x.Id == areaId);
            var pickets = area.Pickets.OrderBy(x => x.Number);
            if (pickets.Count() == 1) 
            {
                return $"{pickets.First().Number}";
            }
            return $"{pickets.First().Number}-{pickets.Last().Number}";
        }
    }
}
