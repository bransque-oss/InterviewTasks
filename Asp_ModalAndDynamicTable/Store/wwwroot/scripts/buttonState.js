function changeBtnState(enabled) {
    console.log('function');
    if (enabled) {
        let attrName = 'disabled';
        let button = document.getElementById('saveBtn');
        if (button.hasAttribute(attrName)) {
            button.removeAttribute(attrName);
        }
    }
}