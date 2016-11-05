; (function ($) {
    $.fn.extend({
        textAreaCount: function (options) {
            var $textArea = this;
            options = $.extend({
                maxlength: 140, // 定义一个最大输入长度变量,初始化为500 
                speed: 15, // 定义删除字符的速度变量 
                msgstyle: "font-family:Arial;font-size:small;color:Gray;small;text-align:right;margin-top:3px;", // 提示信息显示样式 
                msgNumStyle: "font-weight:bold;color:Gray;font-style:italic;font-size:larger;" // 提示信息里面剩余长度的样式 
            }, options);
            var $msg = $("<div style='" + options.msgstyle + "'></div>");
            // 在文本框框后面动态加载一个提示信息容器 
            $textArea.after($msg);
            // 添加keypress事件用来判断当前内容是否还可输入 
            $textArea.keypress(function (e) {
                // 8是Backspace按键， 46是Delete按键 
                // 如果当前可输入的字符长度为0, 且按键值不是8和46，就不做任何操作 
                if ($textArea.val().length >= options.maxlength && e.which != '8' && e.which != '46') {
                    e.preventDefault();
                    return;
                }
            }).keyup(function () { // 添加keyup事件用来计算剩余输入字并显示 
                var curlength = this.value.length;
                $msg.html("").html("还能输入<span style='" + options.msgNumStyle + "'>" + (options.maxlength - curlength) + "</span>字");
                var init = setInterval(function () {
                    // 如果输入的内容大于设置的最大长度，内容按设置的速度自动截取 
                    if ($textArea.val().length > options.maxlength) {
                        $textArea.val($textArea.val().substring(0, options.maxlength));
                        $msg.html("").html("还能输入<span style='" + options.msgNumStyle + "'>" + options.maxlength + "</span>字");
                    }
                    else {
                        clearInterval(init);
                    }
                }, options.speed);
            }).bind("contextmenu", function (e) { // 禁止鼠标右键，防止通过鼠标操作文本 
                return false;
            });
            // 首次加载现在可输入字符长度提示信息 
            $msg.html("").html("还能输入<span style='" + options.msgNumStyle + "'>" + options.maxlength + "</span>字");
            return this;
        }
    });
})(jQuery);