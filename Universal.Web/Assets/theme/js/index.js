$(function(){
	
	
	var ImgSwiper=null,ImgSwiper2=null,ImgSwiper3=null;
	
	
	
	
	
	$('.bsLtTabTop ul li').click(function(){
		var bindex=$('.bsLtTabTop ul li').index(this);
		$(this).addClass('act').siblings().removeClass('act');
		$('.bsLtTabCon .bsTabPt').removeClass('act');
		$('.bsLtTabCon .bsTabPt').eq(bindex).addClass('act');
	});
	
	/*publicTabTop*/
	$('#tab1 span').click(function(){
		var bindex=$('#tab1 span').index(this);
		$(this).addClass('act').siblings().removeClass('act');
		$('#tab1_con .pdULbox').removeClass('act');
		$('#tab1_con .pdULbox').eq(bindex).addClass('act');
	});
	
	$('#tab2 span').click(function(){
		var bindex=$('#tab2 span').index(this);
		$(this).addClass('act').siblings().removeClass('act');
		$('#tab2_con .pdULbox').removeClass('act');
		$('#tab2_con .pdULbox').eq(bindex).addClass('act');
	});
	
	
	/*bsPtLt  bsPtRt*/
	$('.bsPtLt ul li').click(function(){
		var bindex=$('.bsTabPt.act ul li').index(this);
		$(this).addClass('act').siblings().removeClass('act');
		$(this).parents('.bsTabPt').next('.bsPtRt ul').removeClass('act');
		$(this).parents('.bsTabPt').next('.bsPtRt ul').eq(bindex).addClass('act');
	});
	
	$('.stretch').click(function(){
		$('.stretch').removeClass('act');
		$(this).addClass('act');
	});
	
	
	
	
	function setNewsBan(){
		if(w_w>300 && w_w<961){
			getWH();
			if($("#newsImgswiper").length>0){	
				$("#newsImgswiper").width(w_w);
				//$("#newsImgswiper1").width(w_w);
				$("#newsImgswiper,#newsImgswiper .newsImgLi").height(w_w*(310/364));	
				//$("#newsImgswiper1,#newsImgswiper1 .newsImgLi").height(w_w*(310/364));	
				$(".indexNewsImg").height(w_w*(310/364));	
				
			     ImgSwiper = new Swiper('#newsImgswiper',{
					mode:'horizontal',
					autoplay:2000,
					loop:true,
					grabCursor: true,
					resizeReInit : true,
					paginationClickable: true,			
					pagination: '#ImgPagination1'				
				});
				
				ImgSwiper2 = new Swiper('#topiceLinkSwiper',{
					mode:'horizontal',
					autoplay:2000,
					loop:true,
					grabCursor: true,
					resizeReInit : true,
					paginationClickable: true,			
					pagination: '#topiceImgPage'				
				});
				
				
			}
		}else{
			getWH();
								
			$("#newsImgswiper").width(364);
			
			$("#newsImgswiper,#newsImgswiper .newsImgLi").height(310);	
			
			
			$(".indexNewsImg").height(310);						
		     ImgSwiper = new Swiper('#newsImgswiper',{
				mode:'horizontal',
				autoplay:2000,
				loop:true,
				grabCursor: true,
				resizeReInit : true,
				paginationClickable: true,			
				pagination: '#ImgPagination1'				
			});
			
			ImgSwiper2 = new Swiper('#topiceLinkSwiper',{
					mode:'horizontal',
					autoplay:2000,
					loop:true,
					grabCursor: true,
					resizeReInit : true,
					paginationClickable: true,			
					pagination: '#topiceImgPage'				
				});
					
			
		}
	}
	setNewsBan();
	
	var cur2St=0;
	$('.nav li').click(function(){		
		var curLi=$('.nav li').index(this);
		$(this).addClass('act').siblings().removeClass('act');
		$('.homeTab .hBox').removeClass('act');
		$('.homeTab .hBox').eq(curLi).addClass('act');
		
		if(curLi==1){
			
			if(cur2St==0){
				$("#newsImgswiper2").width(364);
				$("#newsImgswiper2,#newsImgswiper1 .newsImgLi").height(310);	
				ImgSwiper2 = new Swiper('#newsImgswiper2',{
					mode:'horizontal',
					autoplay:2000,
					loop:true,
					grabCursor: true,
					resizeReInit : true,
					paginationClickable: true,			
					pagination: '#ImgPagination2'				
				});
				cur2St=1;
			}else{
				ImgSwiper2.reInit();
			}
		}
		
	});
		
	$(window).resize(function(){		
		
	});
	
	
	
	
	
	/*if(w_w>769){
		
	}
	*/
	
	
	
});
