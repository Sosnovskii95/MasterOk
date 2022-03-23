// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var sendImg = new DataTransfer();

function previewImg() {
    var massFileName = document.getElementById("NameImages");
    var outFile = document.getElementById("outFile");

    for (var i = 0; i < massFileName.files.length; i++) {
        var divColImg = document.createElement("div");
        divColImg.setAttribute("class", "col-md-2 d-grid gap-2");
        divColImg.setAttribute("id", massFileName.files[i].name);

        var img = document.createElement("img");
        img.setAttribute("class", "img-thumbnail");

        readFileFromPreview(img, massFileName.files[i]);
        sendImg.items.add(massFileName.files[i]);

        var buttonDelete = document.createElement("input");
        buttonDelete.setAttribute("id", massFileName.files[i].name);
        buttonDelete.setAttribute("type", "button");
        buttonDelete.setAttribute("class", "btn btn-danger btn-sm");
        buttonDelete.setAttribute("value", "Удалить");
        buttonDelete.setAttribute("onclick", "delFileUpload(this.id)");

        divColImg.append(img);
        divColImg.append(buttonDelete);
        outFile.append(divColImg);
    }
    document.getElementById("NameImages").value = "";
}

function readFileFromPreview(img, file) {
    var reader = new FileReader();
    reader.onload = (function (theFile) {
        return function (e) {
            img.setAttribute("src", e.target.result);
        };
    })(file);
    reader.readAsDataURL(file);
}


function delFileUpload(id) {
    document.getElementById(id).remove();

    var massFiles = new DataTransfer();
    for (var i = 0; i < sendImg.files.length; i++) {
        massFiles.items.add(sendImg.files[i]);
    }

    const mass = new DataTransfer();
    var outFile = document.getElementById("outFile").querySelectorAll("div");

    for (var i = 0; i < massFiles.files.length; i++) {
        for (var j = 0; j < outFile.length; j++) {
            if (massFiles.files[i].name == outFile[j].getAttribute("id")) {
                mass.items.add(massFiles.files[i]);
            }
        }
    }

    sendImg = new DataTransfer();
    for (var i = 0; i < mass.files.length; i++) {
        sendImg.items.add(mass.files[i]);
    }
}

function delPreviewImage(id) {
    document.getElementById(id).remove();
}

function setFileUpload() {
    document.getElementById("NameImages").files = sendImg.files;
}
