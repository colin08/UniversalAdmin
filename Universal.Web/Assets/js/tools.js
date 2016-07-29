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
        $(".datacheck").prop("checked",false);
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
function SetPageSize(obj)
{
    var num = $(obj).val();
    var cname = $(obj).attr("cname");
    if (num != "") {
        $.post("/admin/tools/SetPageCookie", { "cname":cname,"num": num }, function (result) {
            //
        });
        //页面刷新
        location.href = '?page=1';
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
        layer.msg('请先选中要删除的数据', { icon: 5 });
        return;
    }
    ids = ids.substring(0, ids.length - 1);
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
                    if (type == 2)
                    {
                        pageData(1);
                    }
                    else
                    {
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