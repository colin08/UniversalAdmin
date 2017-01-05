/*
项目：雷劈网流程设计器
官网：http://flowdesign.leipi.org
Q 群：143263697
基本协议：apache2.0

88888888888  88                             ad88  88                ad88888ba   8888888888   
88           ""                            d8"    88               d8"     "88  88           
88                                         88     88               8P       88  88  ____     
88aaaaa      88  8b,dPPYba,   ,adPPYba,  MM88MMM  88  8b       d8  Y8,    ,d88  88a8PPPP8b,  
88"""""      88  88P'   "Y8  a8P_____88    88     88  `8b     d8'   "PPPPPP"88  PP"     `8b  
88           88  88          8PP"""""""    88     88   `8b   d8'            8P           d8  
88           88  88          "8b,   ,aa    88     88    `8b,d8'    8b,    a8P   Y8a     a8P  
88           88  88           `"Ybbd8"'    88     88      Y88'     `"Y8888P'     "Y88888P"   
                                                          d8'                                
2014-3-15 Firefly95、xinG  
 */

(function($) {

	var defaults = {
		processData : {},// 步骤节点数据
		field_key:{id:'id','title':'node_title'},
		// processUrl:'',//步骤节点数据
		fnRepeat : function() {
			alert("步骤连接重复");
		},
		fnClick : function() {
			alert("单击");
		},
		fnDbClick : function() {
			alert("双击");
		},
		canvasMenus : {
			"one" : function(t) {
				alert('画面右键')
			}
		},
		processMenus : {
			"one" : function(t) {
				alert('步骤右键')
			}
		},
		/* 右键菜单样式 */
		menuStyle : {
			border : '1px solid #5a6377',
			minWidth : '150px',
			padding : '5px 0'
		},
		itemStyle : {
			fontFamily : 'verdana',
			color : '#333',
			border : '0',
			/* borderLeft:'5px solid #fff', */
			padding : '5px 40px 5px 20px'
		},
		itemHoverStyle : {
			border : '0',
			/* borderLeft:'5px solid #49afcd', */
			color : '#fff',
			backgroundColor : '#5a6377'
		},
		mtAfterDrop : function(params) {
			var process_to = $('#window' + params.sourceId).attr(
					'data-process_to');
			var old = process_to ? process_to.split(',') : [];
			var targetId = parseInt(params.targetId);
			for (i = 0; i < old.length; i++) {
				if (old[i] == targetId) {
					return;
				}
			}
			old.push(targetId);
			$('#window' + params.sourceId).attr('data-process_to',
					old.join(','));
		},
		// 这是连接线路的绘画样式
		connectorPaintStyle : {
			lineWidth : 3,
			strokeStyle : "#49afcd",
			joinstyle : "round"
		},
		// 鼠标经过样式
		connectorHoverStyle : {
			lineWidth : 3,
			strokeStyle : "#da4f49"
		}

	};/* defaults end */

	var initEndPoints = function() {
		$(".process-flag").each(function(i, e) {
			var p = $(e).parent();
			jsPlumb.makeSource($(e), {
				parent : p,
				anchor : "Continuous",
				endpoint : [ "Dot", {
					radius : 1
				} ],
				connector : [ "Flowchart", {
					stub : [ 5, 5 ]
				} ],
				connectorStyle : defaults.connectorPaintStyle,
				hoverPaintStyle : defaults.connectorHoverStyle,
				dragOptions : {},
				maxConnections : -1
			});
		});
	}
	var setStyleByItem = function(div, item) {
		var style = {};
		$.each(item, function(k, v) {
			if (/style/gi.test(k) || v == '') {
				return true;
			}
			if (/^left$|^top$/gi.test(k)) {
				style[k] = v + 'px';
			} else if (/^color$/gi.test(k)) {
				style[k] = /^#/gi.test(v) ? v : '#' + v;
			}
			div.attr('data-' + k, v);
		});
		div.css(style);
	}
	/* 设置隐藏域保存关系信息 */
	var aConnections = [];
	var setConnections = function(conn, remove) {
		if (!remove)
			aConnections.push(conn);
		else {
			var idx = -1;
			for (var i = 0; i < aConnections.length; i++) {
				if (aConnections[i] == conn) {
					idx = i;
					break;
				}
			}
			if (idx != -1)
				aConnections.splice(idx, 1);
		}
		if (aConnections.length > 0) {
			var s = "";
			for (var j = 0; j < aConnections.length; j++) {
				var from = $('#' + aConnections[j].sourceId).attr('process_id');
				var target = $('#' + aConnections[j].targetId).attr(
						'process_id');
				s = s + "<input type='hidden' value=\"" + from + "," + target
						+ "\">";
			}
			$('#leipi_process_info').html(s);
		} else {
			$('#leipi_process_info').html('');
		}
		jsPlumb.repaintEverything();// 重画
	};

	/* Flowdesign 命名纯粹为了美观，而不是 formDesign */
	$.fn.Flowdesign = function(options) {
		var _canvas = $(this);
		// 右键步骤的步骤号
		_canvas
				.append('<input type="hidden" id="leipi_active_id" value="0"/><input type="hidden" id="leipi_copy_id" value="0"/>');
		_canvas.append('<div id="leipi_process_info"></div>');

		/* 配置 */
		$.each(options, function(i, val) {
			if (typeof val == 'object' && defaults[i])
				$.extend(defaults[i], val);
			else
				defaults[i] = val;
		});
		//defaults['field_key']=$.extend({id:'id','title':'title'}, defaults.field_key|{});
		
		/* 画布右键绑定 */
		var contextmenu = {
			bindings : defaults.canvasMenus,
			menuStyle : defaults.menuStyle,
			itemStyle : defaults.itemStyle,
			itemHoverStyle : defaults.itemHoverStyle
		}
		$(this).contextMenu('canvasMenu', contextmenu);

		jsPlumb.importDefaults({
			DragOptions : {
				cursor : 'pointer'
			},
			EndpointStyle : {
				fillStyle : '#225588'
			},
			Endpoint : [ "Dot", {
				radius : 1
			} ],
			ConnectionOverlays : [ [ "Arrow", {
				location : 1
			} ], [ "Label", {
				location : 0.1,
				id : "label",
				cssClass : "aLabel"
			} ] ],
			Anchor : 'Continuous',
			ConnectorZIndex : 5,
			HoverPaintStyle : defaults.connectorHoverStyle
		});
		if ($.browser.msie && $.browser.version < '9.0') { // ie9以下，用VML画图
			jsPlumb.setRenderMode(jsPlumb.VML);
		} else { // 其他浏览器用SVG
			jsPlumb.setRenderMode(jsPlumb.SVG);
		}

		// 初始化原步骤
		var lastProcessId = 0;
		var processData = defaults.processData;
		var maxLeft = 0;
		var maxTop = 0;
		
		if (processData.list) {
			$
					.each(
							processData.list,
							function(i, row) {
								var nodeDiv = document.createElement('div');
								var nodeId = "window" + row[defaults.field_key.id], badge = 'badge-inverse', icon = 'fa-star';
								if (lastProcessId == 0)// 第一步
								{
									badge = 'badge-info';
									icon = 'fa-check';
								}
								if (row.icon) {
									icon = row.icon;
								}
								var div = $(nodeDiv)
										.attr("id", nodeId)
										.attr("process_to", row.process_to)
										.attr("process_id", row[defaults.field_key.id])
										.addClass(
												"process-step btn btn-default")
										.html(
												'<span class="process-flag badge '
														+ badge
														+ '"><i class="fa '
														+ icon
														+ '"></i></span>&nbsp;'
														+ row[defaults.field_key.title])
										.mousedown(
												function(e) {
													if (e.which == 3) { // 右键绑定
														_canvas
																.find(
																		'#leipi_active_id')
																.val(row[defaults.field_key.id]);
														contextmenu.bindings = defaults.processMenus
														$(this).contextMenu(
																'processMenu',
																contextmenu);
													}
												});
								setStyleByItem(div,row);
								_canvas.append(nodeDiv);
								// 索引变量
								lastProcessId = row[defaults.field_key.id];
								var top = parseInt($(nodeDiv).css('top'));
								var left = parseInt($(nodeDiv).css('left'));
								if (left > maxLeft) {
									maxLeft = left;
								}
								if (top > maxTop) {
									maxTop = top;
								}
							});// each
		}
		if ($(this).height() < maxTop) {
			$(this).height(maxTop);
		}
		var timeout = null;
		// 点击或双击事件,这里进行了一个单击事件延迟，因为同时绑定了双击事件

		$(document).on(
				"click",
				".process-step",
				function() {
					// 激活
					_canvas.find('#leipi_active_id').val(
							$(this).attr("process_id")), clearTimeout(timeout);
					var obj = this;
					timeout = setTimeout(defaults.fnClick, 300);
				}).on("dblclick", ".process-step", function() {
			clearTimeout(timeout);
			defaults.fnDbClick(this);
		});

		// 使之可拖动
		jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
		initEndPoints();

		// 绑定添加连接操作。画线-input text值 拒绝重复连接
		jsPlumb.bind("jsPlumbConnection", function(info) {
			setConnections(info.connection)
		});
		// 绑定删除connection事件
		jsPlumb.bind("jsPlumbConnectionDetached", function(info) {
			setConnections(info.connection, true);
		});
		// 绑定删除确认操作
		jsPlumb.bind("click", function(c) {
			if (confirm("你确定取消连接吗?"))
				var sourceId = c.source.attr('data-'+defaults.field_key.id);
				var targetId = parseInt(c.target.attr('data-'+defaults.field_key.id));
				var process_to = $('#window' + sourceId).attr('data-process_to');
				var old = process_to ? process_to.split(',') : [];
				var index = this.indexOf(targetId);
				old.splice(index, 1);
				$('#window' + sourceId).attr('data-process_to', old.join(','));
				jsPlumb.detach(c);
		});

		// 连接成功回调函数
		function mtAfterDrop(params) {
			// console.log(params)
			defaults.mtAfterDrop({
				sourceId : $("#" + params.sourceId).attr('process_id'),
				targetId : $("#" + params.targetId).attr('process_id')
			});

		}

		jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
			dropOptions : {
				hoverClass : "hover",
				activeClass : "active"
			},
			anchor : "Continuous",
			maxConnections : -1,
			endpoint : [ "Dot", {
				radius : 1
			} ],
			paintStyle : {
				fillStyle : "#ec912a",
				radius : 1
			},
			hoverPaintStyle : this.connectorHoverStyle,
			beforeDrop : function(params) {
				if (params.sourceId == params.targetId)
					return false;/* 不能链接自己 */
				var j = 0;
				$('#leipi_process_info').find('input').each(
						function(i) {
							var str = $('#' + params.sourceId).attr(
									'process_id')
									+ ','
									+ $('#' + params.targetId).attr(
											'process_id');
							if (str == $(this).val()) {
								j++;
								return;
							}
						})
				if (j > 0) {
					defaults.fnRepeat();
					return false;
				} else {
					mtAfterDrop(params);
					return true;
				}
			}
		});
		// reset start
		var _canvas_design = function() {

			// 连接关联的步骤
			$('.process-step').each(
					function(i) {
						var sourceId = $(this).attr('process_id');
						// var nodeId = "window"+id;
						var prcsto = $(this).attr('process_to');
						var toArr = typeof prcsto == 'undefined' ? [] : prcsto
								.split(",");
						var processData = defaults.processData;
						$.each(toArr, function(j, targetId) {

							if (targetId != '' && targetId != 0) {
								// 检查 source 和 target是否存在
								var is_source = false, is_target = false;
								$.each(processData.list, function(i, row) {
									if (row[defaults.field_key.id] == sourceId) {
										is_source = true;
									} else if (row[defaults.field_key.id] == targetId) {
										is_target = true;
									}
									if (is_source && is_target)
										return true;
								});

								if (is_source && is_target) {
									jsPlumb.connect({
										source : "window" + sourceId,
										target : "window" + targetId
									/*
									 * ,labelStyle : { cssClass:"component
									 * label" } ,label : id +" - "+ n
									 */
									});
									return;
								}
							}
						})
					});
		}// _canvas_design end reset
		_canvas_design();

		// -----外部调用----------------------

		var Flowdesign = {

			addProcess : function(row) {

				if (row[defaults.field_key.id] <= 0) {
					return false;
				}
				var nodeDiv = document.createElement('div');
				var nodeId = "window" + row[defaults.field_key.id], badge = 'badge-inverse', icon = 'fa-star';

				if (row.icon) {
					icon = row.icon;
				}
				$(nodeDiv)
						.attr("id", nodeId)
						.attr("process_to", row.process_to)
						.attr("process_id", row[defaults.field_key.id])
						.addClass("process-step btn btn-default")
						.html(
								'<span class="process-flag badge ' + badge
										+ '"><i class="fa ' + icon
										+ '"></i></span>&nbsp;'
										+ row[defaults.field_key.title])
						.mousedown(
								function(e) {
									if (e.which == 3) { // 右键绑定
										_canvas.find('#leipi_active_id').val(
												row[defaults.field_key.id]);
										contextmenu.bindings = defaults.processMenus
										$(this).contextMenu('processMenu',
												contextmenu);
									}
								});
				setStyleByItem($(nodeDiv),row);
				_canvas.append(nodeDiv);
				// 使之可拖动 和 连线
				jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
				initEndPoints();
				// 使可以连接线
				jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
					dropOptions : {
						hoverClass : "hover",
						activeClass : "active"
					},
					anchor : "Continuous",
					maxConnections : -1,
					endpoint : [ "Dot", {
						radius : 1
					} ],
					paintStyle : {
						fillStyle : "#ec912a",
						radius : 1
					},
					hoverPaintStyle : this.connectorHoverStyle,
					beforeDrop : function(params) {
						var j = 0;
						$('#leipi_process_info').find('input').each(
								function(i) {
									var str = $('#' + params.sourceId).attr(
											'process_id')
											+ ','
											+ $('#' + params.targetId).attr(
													'process_id');
									if (str == $(this).val()) {
										j++;
										return;
									}
								})
						if (j > 0) {
							defaults.fnRepeat();
							return false;
						} else {
							return true;
						}
					}
				});
				return true;

			},
			delProcess : function(activeId) {
				if (activeId <= 0)
					return false;

				$("#window" + activeId).remove();
				return true;
			},
			getActiveId : function() {
				return _canvas.find("#leipi_active_id").val();
			},
			copy : function(active_id) {
				if (!active_id)
					active_id = _canvas.find("#leipi_active_id").val();

				_canvas.find("#leipi_copy_id").val(active_id);
				return true;
			},
			paste : function() {
				return _canvas.find("#leipi_copy_id").val();
			},
			setStyle : function(item) {
				var obj = $('#window' + item.id);
				var style = {};
				$.each(item, function(k, v) {
					if (/^left$|^top$/gi.test(k)) {
						style[k] = v + 'px';
					} else if (/^color$/gi.test(k)) {
						style[k] = /^#/gi.test(v) ? v : '#' + v;
					}
					obj.attr('data-' + k, v);
				});
				obj.css(style);
				$('<i></i>').appendTo(obj.find('span').empty()).addClass(
						'fa ' + item.icon);

			},
			getProcessInfo : function() {
				var list = [];
				_canvas.find("div.process-step").each(function(i) {
					if ($(this).attr('id')) {
						var item = {};
						$.each(this.attributes, function(k, v) {
							if (!/data-/gi.test(v.name)) {
								return true;
							}
							var key = v.name.substr(5);
							item[key] = v.value;
						});
						item.left = parseInt($(this).css('left'));
						item.top = parseInt($(this).css('top'));
						list.push(item);
					}
				});
				return list;
			},
			clear : function() {
				try {

					jsPlumb.detachEveryConnection();
					jsPlumb.deleteEveryEndpoint();
					$('#leipi_process_info').html('');
					jsPlumb.repaintEverything();
					return true;
				} catch (e) {
					return false;
				}
			},
			refresh : function() {
				try {
					// jsPlumb.reset();
					this.clear();
					_canvas_design();
					return true;
				} catch (e) {
					return false;
				}
			}
		};
		return Flowdesign;

	}// $.fn
})(jQuery);