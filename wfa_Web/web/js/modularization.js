


	//错误提示
	function hi(xg){
		$("#hint").fadeIn();
		$("#hint").html(xg)
		setTimeout(function(){
			$("#hint").fadeOut();
		},1500)
	}
	
	
	//字数限制
	function superword(a,b,c){
		$(document).ready(function(){
			var len = $(a).find(b);
			for( var i = 0; i < len.length; i++ ){
				var str = $(len[i]).text();
				if( str.length > c ){
					var num = str.substring(0,c)+"...";
					$(len[i]).html(num)
				}
			}
		})
	}
	
	
	//调用地图
	//[公众号][时间戳][随机字符串][签名][固定点纬度][固定点经度][地理位置名称][位置信息]
	function map(Accounts,time,random,signature,latitude,longitude,designation,message){
		alert(1)
		wx.config({
		    debug: false,
		    appId: Accounts,
		    timestamp: time,
		    nonceStr: random,
		    signature: signature,
		    jsApiList: [
		        // 所有要调用的 API 都要加到这个列表中
		          'getLocation','openLocation'
		      ]
		});
		wx.ready(function (){
		    wx.checkJsApi({
		        jsApiList: [
		            'getLocation'
		        ],
		        
		        success: function (res) {
		            // alert(JSON.stringify(res));
		            // alert(JSON.stringify(res.checkResult.getLocation));
		            if (res.checkResult.getLocation == false) {
		                alert('你的微信版本太低，不支持微信JS接口，请升级到最新的微信版本！');
		                return;
		            }
		        }
		    }); 
		    wx.error(function(res){
		        alert("接口调取失败");
		    });
		
//				显示地图
		 	wx.openLocation({
				latitude: latitude, // 纬度，浮点数，范围为90 ~ -90
				longitude: longitude, // 经度，浮点数，范围为180 ~ -180。
				name: designation, // 位置名
				address: message, // 地址详情说明
				scale: 15, // 地图缩放级别,整形值,范围从1~28。默认为最大
				infoUrl: '' // 在查看位置界面底部显示的超链接,可点击跳转
			}); 
				
			wx.getLocation({
				type : 'gcj02',
				success : function(res){
					var latitude = res.latitude;		//纬度
					var longitude = res.longitude;		//经度
					var speed = res.speed;			//速度
					var accuracy = res.accuracy;	//位置精度
					
				}
			})
		})
	}
	
	
	//绑定滑动事件,并阻止元素发生默认的行为
	//父类块级元素，标识，添加位置
	function iscrollall(parent,identification,loca){
		document.querySelector('.warpped').addEventListener('touchmove', function(e) {
			e.preventDefault();
		});

		var IScroall = new IScroll(document.querySelector('.warpped'), {
			scrollX: false,
			scrollY: true
		});
		//触摸过程中刷新
		$('body').on('touchmove', function() {
			
		})
		//触摸结束时触发
		$('body').bind('touchend', function(){
			var ClientHeight = document.body.clientHeight;	//页面高度
			var len = $(parent).length;		//数量
			var off = $(parent).eq(0);		//第一条
			var last = $(parent).last();	//最后一条
			var OffHeight = off.offset().top;	//第一条距离顶部的距离
			var LastHeight = last.offset().top;	//最后一条距离顶部的距离
			var ide = last.find(identification).val();	//取最后一条标识
			if( OffHeight > 80 ){	//下拉刷新
				
			}
				
			if( ClientHeight - LastHeight > 250 ){	//上滑加载
				console.log(ide+"标识")
				if( len > 9 ){
					$.ajax({
						type:"post",
						url:"[翻页]",
						async:true,
						data: ide,
						success: function(res){
							var Res = res.split("&");
							if( Res[0] == "errcode:0,errmsg:ok" ){
								var tel = Res[1].split("|");
								for( var i = 0; i < tel.length; i++ ){
									if( tel[i] == '' ){
										break;
									}
									var xg = tel[i];
									upglide(xg);
								}
								hi("加载更多");
							}else{
								hi("没有更多了")
							}
							IScroall.refresh();
						},error: function(res){
//							upglide();
//							Error();
							hi("服务器繁忙请稍后重试");
							IScroall.refresh();
						}
					});
				}
				
			}
			
		})
	}

	
	
	
	
	
	
	
	
	
	
	
	
	