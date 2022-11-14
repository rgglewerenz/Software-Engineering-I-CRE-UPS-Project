
function row_load(class_name) {
    var elements = document.getElementsByClassName(class_name);
    for (var i = 0; i < elements.length; i++) {
        do_onto(elements[i]);
    }
}

function do_onto(element) {
    var count = element.childElementCount;
    const width = 1 / count;
    console.log(count);
    console.log(element);
    console.log(width);
    var children = element.children;
    for (var i = 0; i < children.length; i++) {
        change_width(children[i], width);
    }
}

function change_width(element, percentage) {
    console.log(element);
    percentage = percentage * 100;
    console.log(percentage);
    element.style.width = percentage + '%';
    console.log('here');

}
