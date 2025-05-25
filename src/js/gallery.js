document.addEventListener('DOMContentLoaded', (() => {
    document.querySelectorAll('img[gallery]').forEach(x => {
       x.addEventListener('click', function () {
          igl_show(this)
       });
    });
 }));
 function igl_show(img) {
    var iglmodal = document.getElementById('iglmodal');
    var iglmodalImg = document.getElementById('iglmodal-img');
    iglmodal.style.display = 'flex';
    iglmodalImg.src = img.src.replace('_comp','').replace('.webp','.png');
    iglmodalImg.onclick = function (event) {
       event.stopPropagation();
    };
 }
 function igl_hide() {
    document.getElementById('iglmodal').style.display = 'none';
 }
