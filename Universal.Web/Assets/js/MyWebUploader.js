function CreateUploader(options,call_back)
{
    var uploader = WebUploader.create(options);
    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        var $list = $("#fileList"),
                $li = $(
                        '<div id="' + file.id + '" class="file-item thumbnail">' +
                        '<img>' +
                        '<div class="info">' + file.name + '</div>' +
                        '</div>'
                ),
                $img = $li.find('img');


        // $list为容器jQuery实例
        $list.append($li);

        // 创建缩略图
        // 如果为非图片文件，可以不用调用此方法。
        // thumbnailWidth x thumbnailHeight 为 100 x 100
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                return;
            }

            $img.attr('src', src);
        }, 100, 100);
    });

    uploader.addButton({
        id: '#filePicker1'
    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
                $percent = $li.find('.uploader-progress span');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<p class="uploader-progress"><span></span></p>')
                    .appendTo($li)
                    .find('span');
        }

        $percent.css('width', percentage * 100 + '%');
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) {
        // $('#piclist').val(response.name);
        $("#" + file.id).remove();
        console.log(file.id);
        call_back(response);
        //layer.msg(response.data);
        //$(".webupload_current").attr("src",response.data);

        //$('#' + file.id).addClass('upload-state-done');

    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('error', function (code) {
        var msg;
        switch (code) {
            case 'Q_EXCEED_NUM_LIMIT':
                msg = '只能上传' + option.fileNumLimit + '个文件';
                break;
            case 'Q_EXCEED_SIZE_LIMIT':
                msg = '上传文件大小过大';
                break;
            case 'F_EXCEED_SIZE':
                msg = "单个文件不能超过" + fileSingleSizeLimit + "MB";
                break;
            case 'Q_TYPE_DENIED':
                msg = '文件类型上传错误';
                break;
        }
        layer.msg(msg);
    });

    // 文件上传失败，显示上传出错。
    uploader.on('uploadError', function (file) {
        var $li = $('#' + file.id),
                $error = $li.find('div.error');

        // 避免重复创建
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }

        $error.text('上传失败');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.uploader-progress').remove();
    });
}

function addWebuploadCurrent(id) {

    $(".webupload_current").removeClass("webupload_current");
    $("#" + id).addClass("webupload_current");
}