//发送验证码时添加cookie
function addCookie(name, value, expiresHours) {
    //判断是否设置过期时间,0代表关闭浏览器时失效
    if (expiresHours > 0) {
        var date = new Date();
        date.setTime(date.getTime() + expiresHours * 1000);
        $.cookie(name, escape(value), { expires: date });
    } else {
        $.cookie(name, escape(value));
    }
} 

//修改cookie的值
function editCookie(name, value, expiresHours) {
    if (expiresHours > 0) {
        var date = new Date();
        date.setTime(date.getTime() + expiresHours * 1000); //单位是毫秒
        $.cookie(name, escape(value), { expires: date });
    } else {
        $.cookie(name, escape(value));
    }
}
//根据名字获取cookie的值
function getCookieValue(name) {
    return $.cookie(name);
}
$(function () {
    $("#btn_ver").click(function () {
        sendCode($("#btn_ver"));
    });
    v = getCookieValue("secondsremained");//获取cookie值
    if (v > 0) {
        settime($("#btn_ver"));//开始倒计时
    }
})

//发送验证码
function sendCode(obj) {
    var phonenum = $("#txt_telphone").val();
    var result = isPhoneNum();
    if (result) {
        //console.log("类别：" + send_vercode_type);
        mui.showLoading("发送中..", "div");
        mui.ajax('/mp/Tools/SendVer', {
            data: {
                telphone: phonenum,
                type: send_vercode_type
            },
            dataType: 'json',
            type: 'post',
            timeout: 8000,
            async: false,
            headers: { 'Content-Type': 'application/json' },
            success: function (data) {
                mui.hideLoading();
                if (data.msg == 1) {
                    mui.toast(data.msgbox);
                    addCookie("secondsremained", 60, 60);//添加cookie记录,有效时间60s
                    settime(obj);//开始倒计时
                } else {
                    mui.alert(data.msgbox);
                }
            },
            error: function (xhr, type, errorThrown) {
                mui.hideLoading();
                //异常处理；
                mui.alert("请检查联网是否通畅");
            }
        });
        
    }
}
//开始倒计时
var timer;
var countdown;
function settime(obj) {
    countdown = parseInt(getCookieValue("secondsremained"));
    if (countdown == 0 || isNaN(countdown)) {
        obj.removeAttr("disabled");
        obj.removeClass("disenb");
        obj.text("获取验证码");
        return;
    }else{
        obj.text("重新发送(" + countdown + ")");
        obj.attr("disabled", true);
        obj.addClass("disenb");
        countdown--;
        editCookie("secondsremained", countdown, countdown + 1);
    }
    setTimeout(function () { settime(obj) }, 1000) //每1000毫秒执行一次
}
//校验手机号是否合法
function isPhoneNum() {
    var phonenum = $("#txt_telphone").val();
    var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(16[0-9]{1})|(17[0-9]{1})|(19[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
    if (!myreg.test(phonenum)) {
        mui.alert('请输入有效的手机号码');
        return false;
    } else {
        return true;
    }
}

