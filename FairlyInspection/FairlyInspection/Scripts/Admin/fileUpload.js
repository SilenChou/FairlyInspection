$('input[type=file]').change(function () {  //選取類型為file且值發生改變的
    var file = this.files[0]; //定義file=發生改的file
    name = file.name; //name=檔案名稱
    size = file.size; //size=檔案大小
    type = file.type; //type=檔案型態

    if (file.size > 10 * 1024 * 1024) { //假如檔案大小超過300KB (300000/1024)

        alert("圖片上限10MB!!"); //顯示警告!!
        $(this).val("");  //將檔案欄設為空白
    }
    else if (file.type != "image/png" && file.type != "image/jpg" && !file.type != "image/gif" && file.type != "image/jpeg") { //假如檔案格式不等於 png 、jpg、gif、jpeg
        alert("檔案格式不符合: png, jpg or gif"); //顯示警告
        $(this).val(""); //將檔案欄設為空
    }
});