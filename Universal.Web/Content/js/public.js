// JavaScript Document
function getRadioChange(){
	$(".pub_radio").removeClass("pub_radio_hover");
	$("input:radio:checked").each(function(index, element) {
		$(this).siblings(".pub_radio").addClass("pub_radio_hover");
    });
}
function getSelectChange(){
    $(".pub_select_box").click(function(){
		var $this=$(this);
		var $sun=$(this).find(".select_sun_list");
		$sun.toggle();
		$sun.children("a").click(function(){
			$this.find(".inp_select").val($(this).text());
		});
	});
}
function getWindow(){
	if(document.getElementById("minHeight").offsetHeight<600){
		if(600>window.innerHeight-67){
			document.getElementById("minHeight").style.height=600+'px';
		}
		else{
			document.getElementById("minHeight").style.height=(window.innerHeight-67)+'px';
		}
	}
	else{
		if(document.getElementById("minHeight").offsetHeight<window.innerHeight-67){
			document.getElementById("minHeight").style.height=(window.innerHeight-67)+'px';
		}
	}
}
function showOutBox(){
	$("#pub_under_bg").remove();
	var html='<div class="pub_under_bg" id="pub_under_bg" style="height:'+$(window).height()+'px;"></div>';
	$("body").append(html);
	
	$(window).resize(function(){
		$("#pub_under_bg").height($(window).height());
	});
}
$(document).ready(function(e) {
	getWindow();
	$(window).resize(function(){
		getWindow();
	});
	getRadioChange();
	$("input:radio").change(function(){
		getRadioChange();;
	});
	getSelectChange();
	$(".get_num_forInput").keyup(function(){
		var num=$(this).val().length;
		$(this).siblings(".show_num_forInput").html(num+'/150');
		if(num>150){
			$(this).val($(this).val().substr(0,150));
			$(this).siblings(".show_num_forInput").html('150/150');
		}
	});
});