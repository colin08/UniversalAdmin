function isArray(obj)
{
	return Object.prototype.toString.call(obj) === '[object Array]';
};
(function($) {
	$.fn.category=function(options)
	{
		var settings=jQuery.extend({cssName:'form-control', max:0,categorycode:'',name:'',root:0,defText:[],value:0,selected:[],multiple:false},options||{});
		var pool=new $.categoryClassification();
		var _this=this;
		var max=0;
		var length=0;
		this.getValue=function()
		{
			$('#'+settings.name+max);
		};
		this.clear=function(index)
		{
			for(var i=index+1;i<max;i++)
			{
				var sel=$('#'+settings.name+i);
				if(sel.length>0)
				{
					sel.attr('disabled','disabled').hide();
				}
			}
		};
		this.get=function(id)
		{
			return pool.get(id);
		};
		this.getArray=function(item)
		{
			var tmp=[item];
			for(var i=item.depth;i>1;i--)
			{
				item=this.get(item.parent);
				tmp.unshift(item);
			}
			return tmp;
		};
		this.checkcategory=function(item)
		{
			//var _categoryCode={19:[234,248],1:{32:null,33:null}};
			if(!settings.categorycode || settings.categorycode==''){return true;}
			var arr=this.getArray(item);
			var _tmp;
			var _data=settings.categorycode;
			for(var i=0;i<arr.length;i++)
			{
				if(_data==null)
				{
					return true;
				}
				else if(isArray(_data))
				{
					return GetFieldRegex(_data.join(',')).test(arr[i].id);
				}
				else
				{
					_data=_data[arr[i].id];
					if(typeof _data !='object' && typeof _data !='Array'){return false;}
					else if(_data.length<1){return true;}
				}
			}
			return true;
		};
		this.init=function(id,index)
		{
			if(index>=max)
			{
				max=index+1;
			}
			if(index>0 && id==0)
			{
				_this.clear(index-1);
				return;
			}
			_this.clear(index-1);
			var node=pool.get(id);
			if(!node || node.children.length<1)
			{
				return;
			}
			if(settings.max>0 && index>=settings.max){return;}
			var sel=$('#'+settings.name+index);
			if(sel.length<1)
			{
				sel=$('<select name="'+settings.name+index+'"></select>').appendTo(_this).attr('id',settings.name+index).attr('index',index);
				sel.addClass(settings.cssName);
				var onChange=function()
				{
					var _id=parseInt(this.value);
					_this.init(_id,index+1);
					if(typeof settings.selected=='function')
					{
						settings.selected(this,_this);
					}
				};
				if(settings.multiple)
				{
					sel.attr('multiple',true).bind('click',onChange);
				}
				else
				{
					sel.bind('change',onChange);
				}
			}
			sel.attr('disabled',false).show();
			var j=0;
			var defText='';
			if(typeof settings.defText=='string')
			{
				defText=settings.defText;
			}
			else
			{
				defText=settings.defText[index];
			}
			var l=(typeof defText=='undefined' || defText=='')?0:1;
			var options=sel.find('option').remove();
			var add=function(item,i)
			{
				var opt=new Option(item.title,item.id);
				if(item.id==settings.value)
				{
					opt.selected=true;
				}
				if(typeof sel[0].options[i]=='undefined')
				{
					sel[0].options.add(opt);
				}
				else
				{
					sel[0].options[i]=opt;
				}
			};
			if(l>0)
			{
				add({id:0,title:defText},0);
			}
			var _jj=0;
			$(node.children).each(function(i)
			{
				var item=pool.get(this);
				if(typeof item=='undefined')
				{
					return true;
				}
				if(!_this.checkcategory(item)){return true;}
				add(item,_jj+l);
				j++;
				_jj++;
			});
			if(j<1)
			{
				sel.hide();
			}
			else
			{
				_this.clear(index);
				_this.init(parseInt(sel[0].value),index+1);
			}
		};
		this.setValue=function(val)
		{
			if(val>0)
			{
				var _items=[];
				var getrid=function(id)
				{
					var _item=pool.get(id);
					if(typeof _item=='undefined'){return 0;}
					_items.push(_item.id);
					if(_item.parent>=0 && _item.id!=settings.root)
					{
						return getrid(_item.parent);
					}
					return _item.parent;
				};
				getrid(val);				
				if(_items.length>0)
				{
					var j=0;
					for(var i=_items.length-1;i>0;i--)
					{
						settings.value=_items[i-1];
						_this.init(_items[i],j);
						j++;
					}
				}
				else
				{
					_this.init(settings.root,0);
				}
			}
			else
			{
				_this.init(settings.root,0);
			}	
		};
		return this.each(function()
		{
			_this.setValue(settings.value);
		});
	};
	$.categoryClassification = function()
	{
		$.categoryClassification.ids[0]=0;
	};
  $.extend($.categoryClassification,
	{
		items:[{id:0,children:[]}],
		ids:new Object(),
		add:function(data)
		{
			if(typeof this.ids[0]=='undefined')
			{
				this.ids[0]=0;
			}
			var item=$.extend({children:[]},data||{});
			var index=this.items.length;
			this.ids[item.id]=index;
			var par=this.get(item.parent);
			if(typeof par!='undefined')
			{
				par.children.push(item.id);
			}
			this.items.push(item);
		},
		get:function(id)
		{
			return this.items[this.ids[id]];
		},
		prototype:
		{
			get:function(id)
			{
				return $.categoryClassification.get(id);
			}
		}
	});
})(jQuery);
