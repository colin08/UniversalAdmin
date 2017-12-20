// JavaScript Document
/*弹框提示*/
function warmMsgFunc(tp,txt){//tp=1:显示提示语，=2：显示提示语并在两秒后关闭，=其他：删除提示语
	$("#process_page_msg_box").remove();
	if(tp==1){
		$("body").append('<div class="process_page_out_bg" id="process_page_msg_box"><div class="process_page_msg_box process_page_msg_box_tp2"><span>'+txt+'</span></div></div>');
		$("#process_page_msg_box").show();		
	}
	else if(tp==2){
		$("body").append('<div class="process_page_out_bg" id="process_page_msg_box"><div class="process_page_msg_box process_page_msg_box_tp2"><span>'+txt+'</span></div></div>');
		$("#process_page_msg_box").show();
		setTimeout(function(){
			$("#process_page_msg_box").remove();
		},2000);
	}
}
/*弹框确定取消提示*/
function operatingMsgFunc(insertHtml,txtOrt){
	$("#process_page_msg_box").remove();
	var htmlIn='<div class="process_page_out_bg" id="process_page_msg_box"><div class="process_page_msg_box process_page_msg_box_tp1">';
	if(insertHtml==1){//固定搭配，提示语=====警告
		htmlIn+='<div class="txt"><p><em>！</em>'+txtOrt+'</p></div>';
		htmlIn+='<div class="but"><a href="javascript:;" class="process_page_but_public" onclick="$(\'#process_page_msg_box\').remove();">确定</a></div>';
	}
	else if(insertHtml==2){//固定搭配，提示语=====普通提示
		htmlIn+='<div class="txt"><p>'+txtOrt+'</p></div>';
		htmlIn+='<div class="but"><a href="javascript:;" class="process_page_but_public" onclick="$(\'#process_page_msg_box\').remove();">确定</a></div>';
		setTimeout(function(){
			$("#process_page_msg_box").remove();
		},3000);
	}
	else{
		htmlIn+='<div class="txt"><p>'+txtOrt+'</p></div>';
		htmlIn+=insertHtml;
	}
	htmlIn+='</div></div>';
	$("body").append(htmlIn);
	$("#process_page_msg_box").show();
}