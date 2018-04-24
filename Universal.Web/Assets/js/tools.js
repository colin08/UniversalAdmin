//格式化时间
function ChangeDateFormat(cellval, format) {
    try {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var Minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var Second = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        switch (format) {
            case "yyyy-MM-dd":
                return date.getFullYear() + "-" + month + "-" + currentDate;
            case "yyyy-MM-dd HH:mm:ss":
                return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + Minute + ":" + Second;
            default:
                return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + Minute + ":" + Second;
        }
    }
    catch (e) {
        return "";
    }
}

//截取字符串
function CutString(str, len) {
    if (str.length > len) {
        return str.substring(0, len) + "...";
    } else {
        return str;
    }
}

//列表的选中
function CheckAll(obj) {
    if ($(obj).attr("txt") == "全选") {
        $(obj).attr("txt", "取消");
        $(".datacheck").prop("checked", true);
    } else {
        $(obj).attr("txt", "全选");
        $(".datacheck").prop("checked", false);
    }
}

//除法格式化  使用方法：ccRound(size /1024 ,1) + "KB";
function accRound(arg, len) { //len 为保留小数点后几位
    var index = arg.toString().indexOf('.');
    if (index < 0) {
        return arg;
    }
    var r1 = arg.toString().split(".")[1];
    if (r1.length <= len) {
        return arg;
    }
    var r0 = arg.toString().split(".")[0];
    var arg1 = r0 + r1.substr(0, len) + "." + r1.substr(len, 1);
    var arg2 = Math.round(arg1).toString();
    var l = arg2.toString().length - len;
    if (l == 0) {
        return "0." + arg2;
    }
    var arg3 = arg2.substr(0, l) + "." + arg2.substr(l, len);
    return arg3;
}

//转换Byte到KB或MB ，不足1M时显示KB
function ConvertBtteToMB(byteSize) {
    var temp = "";
    byteSize = byteSize / 1024;
    if (byteSize < 1024) {
        temp = accRound(byteSize, 1) + "KB";
    } else {
        temp = accRound(byteSize / 1024, 2) + "MB";
    }
    return temp;
}

//设置分页大小(非Ajax方式)
function SetPageSize(obj) {
    var num = $(obj).val();
    if (parseInt(num) <= 3) {
        layer.msg('单页数据必须大于3', { icon: 5 });
        return;
    }
    var cname = $(obj).attr("cname");
    if (num != "") {
        $.post("/admin/tools/SetPageCookie", { "cname": cname, "num": num }, function (result) {
            //
            location.reload();
        });
        //location.href = '?page=1';
    }
}

//列表删除数据 type=1：完成后刷新整页，type=2：完成后Ajax重新加载
function del(url, type) {
    var ids = "";
    $(".datacheck").each(function () {
        if ($(this).is(':checked')) {
            ids = ids + $(this).attr("hid") + ","
        }
    })
    if (ids == "") {
        layer.msg('请先选中要操作的数据', { icon: 5 });
        return;
    }
    ids = ids.substring(0, ids.length - 1);
    layer.confirm('确认操作？', { icon: 3 }, function (index) {
        layer.close(index);
        $.ajax({
            type: "post",
            url: url,
            data: { "ids": ids },
            async: false,
            beforeSend: function () {

            },
            complete: function () {

            },
            success: function (data) {
                if (data.msg == 1) {
                    layer.msg(data.msgbox, { icon: 1 });
                    if (type == 2) {
                        pageData(1);
                    }
                    else {
                        window.location.reload();
                    }
                }
                else {
                    layer.msg(data.msgbox, { icon: 2 });
                }
            },
            error: function () {
                layer.msg("操作失败，请检查网络", { icon: 5 });
            }
        })
    });
}

//列表指定删除某条数据 type=1：完成后刷新整页，type=2：完成后Ajax重新加载
function del_item(url,ids, type) {
    if (ids == "") {
        layer.msg('不知道删除哪一个', { icon: 5 });
        return;
    }
    layer.confirm('将同时删除所关联的数据，且不可恢复，是否继续？', { icon: 3 }, function (index) {
        layer.close(index);
        $.ajax({
            type: "post",
            url: url,
            data: { "ids": ids },
            async: false,
            beforeSend: function () {

            },
            complete: function () {

            },
            success: function (data) {
                if (data.msg == 1) {
                    layer.msg(data.msgbox, { icon: 1 });
                    if (type == 2) {
                        pageData(1);
                    }
                    else {
                        window.location.reload();
                    }
                }
                else {
                    layer.msg(data.msgbox, { icon: 2 });
                }
            },
            error: function () {
                layer.msg("操作失败，请检查网络", { icon: 5 });
            }
        })
    });
}

function DelPic(obj, hide_name) {
    //var pic = $(obj).parent("li").find("img").attr("src");
    $(obj).parent("li").remove();
    if (hide_name.length > 0) {
        var list_name = $(obj).parent("li").find("img").attr("class");
        if ($("." + list_name + "").size() > 0) {
            var file_name = "";
            $("." + list_name + "").each(function (index, elem) {
                file_name += $(this).attr("src") + ",";
            })
            if (file_name.length > 0) {
                file_name = file_name.substring(0, file_name.length - 1);
            }
            $("#" + hide_name + "").val(file_name);
        }
        else {
            $("#" + hide_name + "").val("");
        }
    }
    //$("#" + up_id + "").uploadifive('clearQueue')
    //$("#"+up_id+"").uploadify('disable', false) //浏览按钮可用
}

//Loading 层
function configm_before() {
    if ($("form").valid())
    {
        layer.load(2);
    }
}

function InitTextArea(TextAreaID)
{
    /** 富文本编辑器 begin  **/
    var toolbar = ['title', 'bold', 'italic', 'underline', 'strikethrough', 'fontScale', 'color', '|', 'ol', 'ul', 'blockquote', 'code', '|', 'link', 'image', 'hr', '|', 'indent', 'outdent', 'alignment', 'markdown'];
    var editor = new Simditor({
        textarea: $('#' + TextAreaID + ''),
        autosave: 'editor-'+TextAreaID,
        toolbar: toolbar,
        defaultImage: '/Assets/img/default_avatar.jpg', //编辑器插入图片时使用的默认图片  
        upload: {
            url: '/Admin/Tools/UploadAction',
            params: { operation: "area", upload_type: "TxtArea" },
            fileKey: 'imgFile', //服务器端获取文件数据的参数名  
            connectionCount: 3,
            leaveConfirm: '正在上传文件',
        },
        success: function (data) {
            console.log(data);
            //alert(data);
        }
    });

    /** 富文本编辑器 end  **/
}

//文件下载
function FileDown(uri,name)
{
    var data = "uri=" + uri + "&name=" + name;
    FileDownLoad("/File/DownLoad", data, "post");
}

function FileDownLoad(url, data, method) {
    // 获得url和data
    if (url && data) {
        // data 是 string 或者 array/object
        data = typeof data == 'string' ? data : jQuery.param(data);
        // 把参数组装成 form的  input
        var inputs = '';
        jQuery.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        // request发送请求
        jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
        .appendTo('body').submit().remove();
    };
};

function KeyDown(funcStr) {
    if (event.keyCode == 13) {
        eval(funcStr);
    }
}

// Ajax 文件下载
//jQuery.download = function (url, data, method) {
//    // 获得url和data
//    if (url && data) {
//        // data 是 string 或者 array/object
//        data = typeof data == 'string' ? data : jQuery.param(data);
//        // 把参数组装成 form的  input
//        var inputs = '';
//        jQuery.each(data.split('&'), function () {
//            var pair = this.split('=');
//            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
//        });
//        // request发送请求
//        jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
//        .appendTo('body').submit().remove();
//    };
//};