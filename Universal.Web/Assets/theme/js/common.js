
String.prototype.format = function(k, v) {
	return this.replace(new RegExp('\{' + k + '\}', 'gm'), v);
};
if (!('incstr' in String.prototype)) {
	String.prototype.incstr = function(s, l) {
		var tmp = this;
		for (var i = tmp.length; i < l; i++) {
			tmp = s + "" + tmp;
		}
		return tmp;
	}
}

Date.prototype.toString = function(t) {
	if (t) {
		var dt = new Date();
		var tmp = "";
		t = t.replace(/day/g, this - dt);
		t = t.replace(/age/g, dt.getFullYear() - this.getFullYear());
		t = t.replace(/yyyy/g, this.getFullYear());
		t = t.replace(/yy/g, (this.getFullYear() + '').substr(2, 2));
		t = t.replace(/MM/g, String(this.getMonth() + 1).incstr('0', 2));
		t = t.replace(/M/g, this.getMonth() + 1);
		t = t.replace(/dd/g, String(this.getDate()).incstr('0', 2));
		t = t.replace(/d/g, this.getDate());
		t = t.replace(/hh/gi, String(this.getHours()).incstr('0', 2));
		t = t.replace(/h/gi, this.getHours());
		t = t.replace(/mm/g, String(this.getMinutes()).incstr('0', 2));
		t = t.replace(/m/g, this.getMinutes());
		t = t.replace(/ss/g, String(this.getSeconds()).incstr('0', 2));
		t = t.replace(/s/g, this.getSeconds());
		t = t.replace(/w/g, this.getDay());
		return t;
	} else {
		return this.getFullYear() + "-" + (this.getMonth() + 1) + "-"
				+ this.getDate();
	}
}

var w_w = 0, w_h =0, d_w =0,down_data={};
function getWH() {
	w_w = $(window).width();
	d_w = $(document).width();
	w_h = $(window).height();
}

$(function() {
	getWH();
	$('.toplink ul li').hover(function() {
		if ($(this).find('.lipop').length > 0) {
			$(this).find('.lipop').show();
		}
	}, function() {
		if ($(this).find('.lipop').length > 0) {
			$(this).find('.lipop').hide();
		}
	});

	$('.menlist ul li').click(function() {
		if ($(this).hasClass('act')) {
			$(this).removeClass('act');
		} else {
			$(this).addClass('act');
		}
	});
	// 不能单纯移除，还要做数据处理 by pigeon
	// $('.menlist ul li .i-mdel').click(function() {
	// $(this).parents('.menlist ul li').remove();
	// });

	/* 移动端 */
	if (w_w < 960) {
		/* 菜单 */
		/*
		 * $(".m-menu").on('click',function(){ if($(this).hasClass("act")){
		 * $(this).removeClass("act"); $(".nav").hide(); }else{
		 * $(this).addClass("act"); $(".nav").show(); } });
		 * 
		 * $("body").bind("click", function (e) { var target = $(e.target); if
		 * (target.closest(".header").length == 0) { $(".nav").hide(); } })
		 */
	} else {
	}

	var curH = w_h - 67;
	$('.rightBox,.leftMenu,.fade').height(curH);
	$('.fade').height(w_h);

	/* 下拉菜单 大 */
	if ($(".selDiv").length > 0) {
		$(".selDiv i,.selDiv span").click(
				function(e) {
					$(".selDiv ul").hide(0);
					$(".selDiv").removeClass('zH');
					$(this).parents(".selDiv").addClass('zH');
					$(this).parents(".selDiv").find("ul").stop(true, true)
							.slideDown(200);
					e.stopPropagation();
				});

		$(".selDiv ul li").click(function(e) {
			var str = $(this).html();
			$(this).parents(".selDiv").find("span").html(str);
			$(this).parents(".selDiv").find("ul").stop(true, true).hide(0);
			$(".selDiv2,.selDiv").removeClass('zH');
			e.stopPropagation();
		});
	}
	$(document.body).click(function(e) {
		$(".selDiv ul").hide();
	});
	$(window).resize(function() {
		getWH();
		curH = w_h - 67;
		$('.rightBox,.leftMenu').height(curH);
		$('.fade').height(w_h);
	});

	$('.user').hover(function() {
		$(this).find('.userPop').show();
	}, function() {
		$(this).find('.userPop').hide();
	});

	$('.a-close').click(function() {
		$('.pop,.fade,.smTips').hide();
	});

	$('.fileLt li h5').click(function() {
		if ($(this).parents('li').hasClass('act')) {
			$(this).parents('li').removeClass('act');
		} else {
			$(this).parents('li').addClass('act');
		}

	});

});

function setIndexImgH() {
	/* 针对笔记本高度适配 */
	// document.title=w_h;
}
function hideDialog(id) {
	if (typeof id == 'undefined') {
		id = 'dialog_box';
	}
	$('#' + id).hide();

	if ($('.pop:visible').size() < 1) {
		$('.mask').hide();
	}
}
function showDialog(data) {
	data = $.extend({
		id : 'dialog_box',
		title : '',
		html : '',
		css : '',
		page : {},
		width : 0,
		closeMask : true,
		onshow : function() {
		},
		onOk : false,
		onCancel : false,
		fix:false,
		buttons : []
	}, data || {});
	var box = $('#' + data.id);
	if (box.size() < 1) {
		box = $('<div></div>').appendTo($('body')).addClass('pop').attr('id',
				data.id);
		data.page.top = $('<div></div>').appendTo(box).addClass('popTop');
		$('<a href="javascript:"></a>').appendTo(data.page.top).attr('title',
				'关闭弹出框').addClass('a-close').click(function() {
			hideDialog(data.id, data.closeMask);
		});
		data.page.title = $('<b></b>').appendTo(data.page.top);
		data.page.main = $('<div></div>').appendTo(box).addClass('popMid');
		data.page.footer = $('<div></div>').appendTo(box)
				.addClass('pop-footer').hide();
	} else {
		data.page.top = box.find('.popTop');
		data.page.title = data.page.top.find('b');
		data.page.main = box.find('.popMid');
		data.page.footer = box.find('.pop-footer');

	}
	box.removeClass('').addClass('pop');
	if (data.css != '') {
		box.addClass(data.css);
	}
	data.page.mask = $('.mask');
	if (data.page.mask.size() < 1) {
		data.page.mask = $('<div></div>').appendTo($('body')).addClass('mask');
	}
	data.page.title.html(data.title);
	$(data.html).appendTo(data.page.main.empty());
	if (data.width > 0) {
		box.css({
			width : data.width + 'px',
			marginLeft : '-' + (data.width / 2) + 'px'
		});
	}
	if (typeof data.onOk == 'function' && data.buttons.length == 0) {
		data.buttons.push({
			text : '确认',
			cssName : 'Btn2 red mr_15',
			onclick : data.onOk
		});
		data.buttons.push({
			text : '取消',
			cssName : 'Btn2',
			onclick : function() {
				hideDialog(data.id);
			}
		});
	}
	data.page.footer.empty();
	$(data.buttons).each(function() {
		var that = this;
		var btn = $('<button></button>').appendTo(data.page.footer);
		btn.text(this.text);
		btn.addClass(this.cssName);
		btn.click(function() {
			that.onclick(this, box);
		})
	});
	if (data.buttons.length > 0) {
		data.page.footer.show();
	}
	box.show();
	var fixPostion = function() {
		if (box.is(':hidden')) {
			return;
		}
		var top = ($(window).height() - box.height()) / 2
				+ $(document).scrollTop();
		box.css({
			'marginTop' : top + 'px',
			top : 0
		});
		setTimeout(function() {
			fixPostion();
		}, 100);
	}
	if(data.fix){
	 setTimeout(function() {
			fixPostion();
		}, 100);
	}

	data.page.mask.show();
	data.onshow(data);
}

function getUserList(data) {
	data = $.extend({
		keyword : '',
		callback : function() {
		}
	}, data || {});
	$.ajax({
		url : '/Tools/GetUserList?search=' + data.keyword,
		success : function(ret) {
			data.callback(ret);
		}
	})
}

function selUserDialog(data) {
	data = $.extend({
		keyword : '',
		max : 500,
		count : 0,
		items : {},
		callback : function() {
		}
	}, data || {});

	var set_count = function() {
		$('#sel_user_box_seled').html('已选择成员：' + data.count + '/' + data.max);
	}
	var remove_user = function(id) {
		var ul = $('#sel_user_box_ul');
		ul.find('#user_item_' + id).remove();
		$('#sel_user_box_dl2').find('#user_data_' + id).find('input').attr(
				'checked', false);
		data.count--;
		delete data.items[id];
		set_count();
	}
	var sel_user = function(obj, item) {
		var ul = $('#sel_user_box_ul');
		if (typeof obj.checked == 'undefined' || obj.checked == false) {
			remove_user(item.id);
			return;
		}
		var li = $('<li></li>').appendTo(ul).attr('id', 'user_item_' + item.id);
		var a = $('<a></a>').appendTo(li).click(function() {
			remove_user(item.id);
		});
		$('<span></span>').appendTo(a).html(
				item.telphone + ' (' + item.nick_name + ')');
		$('<i class="i-mdel"></i>').appendTo(a);
		data.count++;
		set_count();
		data.items[item.id] = item;
	}
	var getList = function(key) {
		$.ajax({
			url : '/Tools/GetUserList?search=' + key,
			success : function(ret) {
				$('#sel_user_box_h52').html('成员列表(' + ret.total + '人)');
				var dl = $('#sel_user_box_dl2').empty();
				$(ret.data).each(
						function(i, item) {
							var dd = $('<dd></dd>').appendTo(dl).attr('id',
									'user_data_' + this.id)
							$('<label></label>').appendTo(dd).html(
									this.telphone + '(' + this.nick_name + ')')
									.attr('for', 'u_list_id_' + this.id)
							$('<input type="checkbox" />').appendTo(dd).attr(
									'id', 'u_list_id_' + this.id).val(this.id)
									.click(function() {
										sel_user(this, item);
									});
						});

			}
		});
	};
	var mask = $('.mask');

	var switchTab = function(obj) {
		if (obj == 1) {
			$('#sel_user_box .smbTabCon').find('.smbpt').removeClass('act');
			$('#sel_user_box .smbTabTop').find('span').removeClass('act');
			$('#sel_user_box .smbTabCon').find('.smbpt').eq(obj)
					.addClass('act');
			$('#sel_user_box .smbTabTop').find('span').eq(obj).addClass('act');
		}
		var p = $(obj).parent();
		p.find('.act').removeClass('act');
		$(obj).addClass('act');
		var i = p.find('span').index($(obj));
		$('#sel_user_box .smbTabCon').find('.act').removeClass('act');
		$('#sel_user_box .smbTabCon').find('.smbpt').eq(i).addClass('act');
	}
	var box = $('<div class="selMemberBox"></div>');
	var smbLt = $('<div class="smbLt"></div>').appendTo(box);

	var smbSearch = $('<div class="smbSearch"></div>').appendTo(smbLt);
	$(
			'<input type="text" id="sel_user_box_key" name="key" placeholder="搜索电话号码或姓名" />')
			.appendTo(smbSearch).keypress(function() {
				if (event.keyCode == 13) {
					getList($('#sel_user_box_key').val());
					switchTab(1);
				}
			});
	$('<input type="button" />').appendTo(smbSearch).click(function() {
		getList($('#sel_user_box_key').val());
		switchTab(1);
	});

	var smbTab = $('<div class="smbTab"></div>').appendTo(smbLt);
	var smbTabTop = $('<div class="smbTabTop"></div>').appendTo(smbTab);
	$('<span class="act">最近</span>').appendTo(smbTabTop).click(function() {
		switchTab(this);
	})
	$('<span>企业通讯录</span>').appendTo(smbTabTop).click(function() {
		switchTab(this);
	})

	var smbTabCon = $('<div class="smbTabCon"></div>').appendTo(smbTab);
	var smbpt1 = $('<div class="smbpt act"></div>').appendTo(smbTabCon);
	var list1 = $('<div class="smbLtlist"></div>').appendTo(smbpt1);
	var ul = $('<ul></ul>').appendTo(list1);
	var li = $('<li></li>').appendTo(ul);
	var titl = $('<h5>最近联系人</h5>').appendTo(li);
	var dl = $('<dl class="smbSec"></dl>').appendTo(li);
	if (typeof sessionStorage.last_sel_user != 'undefined') {
		var tmp = {};
		tmp = $.parseJSON(sessionStorage.last_sel_user);
		var j = 0;
		var items = [];
		$.each(tmp, function() {
			items.push(this);
		});
		items.sort(function(a, b) {
			if (a.time < b.time) {
				return 1;
			} else if (a.time > b.time) {
				return -1;
			} else {
				return 0;
			}
		});
		$(items).each(
				function() {
					j++;
					var item = this;
					var dd = $('<dd></dd>').appendTo(dl);
					$('<label></label>').appendTo(dd).html(
							this.telphone + ' (' + this.nick_name + ')').attr(
							'for', 'l_user_id_' + this.id);
					$('<input type="checkbox">').appendTo(dd).val(this.id)
							.attr('id', 'l_user_id_' + this.id).click(
									function() {
										sel_user(this, item);
									});
				});
		titl.html('最近联系人 (' + j + '人)</h5>');
	}

	var smbpt2 = $('<div class="smbpt"></div>').appendTo(smbTabCon);
	var list2 = $('<div class="smbLtlist"></div>').appendTo(smbpt2);
	var ul = $('<ul></ul>').appendTo(list2);
	var li = $('<li></li>').appendTo(ul);
	$('<h5 id="sel_user_box_h52"></h5>').appendTo(li).html('成员列表');
	$('<dl class="smbSec" id="sel_user_box_dl2"></dl>').appendTo(li);

	// document.write(typeof sessionStorage.lastname);

	var smbRt = $('<div class="smbRt"></div>').appendTo(box);
	$('<div class="smbInput" style="height:35px;">&nbsp;</div>')
			.appendTo(smbRt);
	$('<div class="clear"></div>').appendTo(smbRt);
	var smbList = $('<div class="smbList"></div>').appendTo(smbRt);
	$('<h5 id="sel_user_box_seled"></h5>').appendTo(smbList);

	var div = $('<div class="smbListBox"></div>').appendTo(smbList);
	$('<ul id="sel_user_box_ul"></ul>').appendTo(div);

	$('<div class="clear"></div>').appendTo(box);
	var foot = $('<div class="mt_20 tr"></div>').appendTo(box);
	$('<button type="button" class="Btn2 red mr_15">确定</button>')
			.appendTo(foot).click(function() {
				// alert(JSON.stringify(data.items));
				var items = [];
				var tmp = {};
				if (typeof sessionStorage.last_sel_user != 'undefined') {
					tmp = $.parseJSON(sessionStorage.last_sel_user);
				}
				$.each(data.items, function() {
					items.push(this);
					this.time = new Date().getTime();
					tmp[this.id] = this;
				});
				sessionStorage.last_sel_user = JSON.stringify(tmp);
				// alert(JSON.stringify(items));
				data.callback(items);
				hideDialog('sel_user_box');
			});
	$(
			'<button type="button" class="Btn2" onclick="hideDialog(\'sel_user_box\')">取消</button>')
			.appendTo(foot);

	showDialog({
		id : 'sel_user_box',
		title : '选择成员',
		css : 'selMember',
		html : box
	});
	set_count();
	getList('');
}

function showSelectBox(data) {
	data = $.extend({
		id : 'show_select_box',
		title : '请选择',
		head_title : '列表',
		width : 400,
		maxHeight : 350,
		onOk : function(ret) {
			alert(ret.name);
		},
		items : []
	}, data || {});
	var box = $('<div class="vi-selector"></div>');
	$('<h5 class="vi-selector-title"></h5>').appendTo(box)
			.html(data.head_title);
	var div = $('<div class="vi-selector-main"></div>').appendTo(box);
	div.css({
		maxHeight : data.maxHeight + 'px'
	});
	var ul = $('<ul class="vi-selector-radio"></ul>').appendTo(div);

	var addItem = function(box, items, depth) {
		$(items).each(
				function() {
					var li = $('<li></li>').appendTo(box).addClass(
							'depth' + depth);
					var label = $('<label></label>').appendTo(li).html(
							this.name).attr('for', data.id + '_' + this.value);
					$('<input type="radio" />').appendTo(li).val(this.value)
							.attr('id', data.id + '_' + this.value).attr(
									'name', data.id).attr('data-name',
									this.name);
					if (typeof this.children != 'undefined'
							&& this.children.length > 0) {
						// var ul1=$('<ul></ul>').appendTo(li);
						$('<em></em>').prependTo(li);
						addItem(ul, this.children, depth + 1);
					}
				});
	}
	addItem(ul, data.items, 1);
	// $(data.items).each(function(){
	// var li=$('<li></li>').appendTo(ul);
	// var
	// label=$('<label></label>').appendTo(li).html(this.name).attr('for',data.id+'_'+this.value);
	// $('<input type="radio"
	// />').appendTo(label).val(this.value).attr('id',data.id+'_'+this.value).attr('name',data.id);
	//		
	// });
	var onOk = function(obj, box) {
		var input = $('.vi-selector-radio input[name=' + data.id + ']:checked');
		if (input.size() < 1) {
			alert('请选择');
			return;
		}
		hideDialog(data.id);
		data.onOk({
			value : input.val(),
			name : input.attr('data-name')
		});
	}
	showDialog({
		id : data.id,
		width : data.width,
		title : data.title,
		css : '',
		html : box,
		onOk : onOk
	});
}
// $('body').ready(function(){
// showSelectBox({items:[
// {value:1,name:'男'},
// {value:2,name:'女'}
// ]});
// });
function addDownUrl(data) {
	// <a href="javascript:;" onclick="down('@file.FilePath','@item.Title
	// @file.FileName')"><i class="i-downL"></i></a>
	var box = $('#doc_' + data.id);
	if (box.length < 1) {
		return;
	}
	var a = box.find('.down_url');
	if (a.length < 1) {
		down_data[data.id] = [];
		a = $('<a></a>').appendTo(box).attr('href', 'javascript:;').attr(
				'data-id', data.id).click(function() {
			showDownDialog(this);
		});
		$('<i class="i-downL"></i>').appendTo(a);
	}
	down_data[data.id].push(data);
}
function showDownDialog(obj,data) {
	if(data){
		item=data;
	}else{
		var id = $(obj).attr('data-id');
		var item = down_data[id];
	}
	console.log(JSON.stringify(item));
	var div=$('<div></div>').addClass('down_dialog');
	var ul=$('<ul></ul>').appendTo(div);
	$(item).each(function(){
		var li=$('<li></li>').appendTo(ul);
		var a=$('<a></a>').appendTo(li).attr('href',this.FilePath);
		$('<i></i>').appendTo(a);
		$('<span></span>').appendTo(a).html(this.FileName);
		$('<label></label>').appendTo(a).html(this.FileSize);
	});
	showDialog({title:'下载秘籍',html:div,fix:true});
}


//截取字符串
function CutString(str, len) {
    if (str == null || str == undefined) {
        return "";
    }
    if (str.length > len) {
        return str.substring(0, len) + "...";
    } else {
        return str;
    }
}
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
                return date.getFullYear().toString().substring(2) + "-" + month + "-" + currentDate + " " + hour + ":" + Minute;
        }
    }
    catch (e) {
        return "";
    }
}

//列表的选中
function CheckAll(obj) {
    if ($(obj).is(':checked')) {
        $(".all_checked_but").prop("checked", true);
    } else {
        $(".all_checked_but").prop("checked", false);
    }
}

//转换Byte到KB或MB ，不足1M时显示KB
function ConvertBtteToMB(byteSize) {
    if (byteSize <= 0) {
        return "0";
    }
    var temp = "";
    byteSize = byteSize / 1024;
    if (byteSize < 1024) {
        temp = accRound(byteSize, 1) + "KB";
    } else {
        temp = accRound(byteSize / 1024, 2) + "MB";
    }
    return temp;
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