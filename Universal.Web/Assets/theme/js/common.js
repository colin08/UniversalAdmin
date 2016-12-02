// JavaScript Document


/*if(navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.match(/7./i)=="7.") 
{ 
	
} 
else if(navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.match(/8./i)=="8.") 
{ 
	
} 
else if(navigator.appName == "Microsoft Internet Explorer") 
{ 
	$("#broweTips,.fade").show();	
} 

$(".aClose").click(function(){
	$("#broweTips,.fade").hide();	
});*/

// JavaScript Document

/*首页脚本*/
var w_w=$(window).width(),w_h=$(window).height(),d_w=$(document).width();;
function getWH(){
	w_w=$(window).width();
	d_w=$(document).width();
	w_h=$(window).height();
}

$(function(){
	
	$('.toplink ul li').hover(function(){
		if($(this).find('.lipop').length>0){
			$(this).find('.lipop').show();
		}
	},function(){
		if($(this).find('.lipop').length>0){
			$(this).find('.lipop').hide();
		}
	});
	
	$('.menlist ul li').click(function(){
		if($(this).hasClass('act')){
			$(this).removeClass('act');
		}else{
			$(this).addClass('act');
		}
	});
	
	$('.menlist ul li .i-mdel').click(function(){
		$(this).parents('.menlist ul li').remove();
	});
	
        
    /*移动端*/
	 if(w_w<960){
		 /*菜单*/
		/*$(".m-menu").on('click',function(){
			if($(this).hasClass("act")){			
					$(this).removeClass("act");			
					$(".nav").hide();
				}else{
					$(this).addClass("act");
					$(".nav").show();
			}
		});
		
		$("body").bind("click", function (e) {
			var target = $(e.target);
			if (target.closest(".header").length == 0) {
				$(".nav").hide();
			}
		})*/
	}else{}
	
	
	var curH=w_h-67;
	$('.rightBox,.leftMenu,.fade').height(curH);
	$('.fade').height(w_h);
	
	/*下拉菜单 大*/
	if($(".selDiv").length>0){
		$(".selDiv i,.selDiv span").click(function(e){
			$(".selDiv ul").hide(0);
			$(".selDiv").removeClass('zH');
			$(this).parents(".selDiv").addClass('zH');
			$(this).parents(".selDiv").find("ul").stop(true,true).slideDown(200);
			e.stopPropagation();
		});	
		
		 $(".selDiv ul li").click(function(e){
			var str=$(this).html();	
			$(this).parents(".selDiv").find("span").html(str);
			$(this).parents(".selDiv").find("ul").stop(true,true).hide(0);
			$(".selDiv2,.selDiv").removeClass('zH');
			e.stopPropagation();
		}); 
	}
	$(document.body).click(function (e) {
		$(".selDiv ul").hide();
	})
	
	$(window).resize(function(){
		getWH();	
		curH=w_h-67;
		$('.rightBox,.leftMenu').height(curH);
		$('.fade').height(w_h);
	});
	
	$('.user').hover(function(){
		$(this).find('.userPop').show();
	},function(){
		$(this).find('.userPop').hide();
	});
	
	$('.a-close').click(function(){
		$('.pop,.fade,.smTips').hide();
	});
	
	$('.fileLt li h5').click(function(){
		if($(this).parents('li').hasClass('act')){
			$(this).parents('li').removeClass('act');
		}else{
			$(this).parents('li').addClass('act');
		}
		
	});
	

});

function setIndexImgH(){
	/*针对笔记本高度适配*/
	//document.title=w_h;

	

}

