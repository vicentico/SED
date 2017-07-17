/*!
 * dialogBox v0.0.1
 * by Liuyuchao
 * Copyright 2015.3
 * Date: Wed Mar 25 2015
 */

;
(function($, window, document) {

	var pluginName = "dialogBox",
		defaults = {
			width: null, //弹出层宽度
			height: null, //弹出层高度
			autoSize: true, //是否自适应尺寸,默认自适应
			autoHide: false, //是否自自动消失，配合time参数共用
			time: 3000, //自动消失时间，单位毫秒
			zIndex: 99999999, //弹出层定位层级
			hasMask: false, //是否显示遮罩层
			hasClose: false, //是否显示关闭按钮
			hasBtn: false, //是否显示操作按钮，如取消，确定
			confirmValue: null, //确定按钮文字内容
			confirm: function() {}, //点击确定后回调函数
			cancelValue: null, //取消按钮文字内容
			cancel: function() {}, //点击取消后回调函数，默认关闭弹出框
			effect: "", //动画效果：fade(默认),newspaper,fall,scaled,flip-horizontal,flip-vertical,sign,
			type: "", //对话框类型：normal(普通对话框),correct(正确/操作成功对话框),error(错误/警告对话框)
			title: "", //标题内容，如果不设置，则连同关闭按钮（不论设置显示与否）都不显示标题
			content: "" //正文内容，可以为纯字符串，html标签字符串，以及URL地址，当content为URL地址时，将内嵌目标页面的iframe。

		};

	function dialogBox(element, options) {
		this.element = element;
		this.settings = $.extend({}, defaults, options);
		this.init();
	}

	dialogBox.prototype = {

		//初始化弹出框
		init: function() {
			var that = this,
				element = this.element;

			that.render(element);
			that.setStyle();
			that.show();
			that.trigger(element);
		},

		//创建弹出框
		create: function(element) {
			var that = this,
				$this = $(element),
				title = that.settings.title,
				hasBtn = that.settings.hasBtn,
				hasMask = that.settings.hasMask,
				hasClose = that.settings.hasClose,
				confirmValue = that.settings.confirmValue,
				cancelValue = that.settings.cancelValue,
				dialogHtml = [];


			if (!title) {
				dialogHtml[0] = "<section id=\"dialog-box\"><div class=\"dialog-box-container\"><div class=\"dialog-box-content\"></div>";
			} else {
				if (!hasClose) {
					dialogHtml[0] = "<section id=\"dialog-box\"><div class=\"dialog-box-container\"><div class=\"dialog-box-title\"><h3>" + title + "</h3></div><div class=\"dialog-box-content\"></div>";
				} else {
					dialogHtml[0] = "<section id=\"dialog-box\"><div class=\"dialog-box-container\"><div class=\"dialog-box-title\"><h3>" + title + "</h3><span class=\"dialog-box-close\">×</span></div><div class=\"dialog-box-content\"></div>";
				}
			}

			if (!hasBtn) {
				dialogHtml[1] = "</div></section>";
			} else {
				if (confirmValue && cancelValue) {
					dialogHtml[1] = "<div class=\"dialog-btn\"><span class=\"dialog-btn-confirm pull-left\">" + confirmValue + "</span><span class=\"dialog-btn-cancel  pull-right\">" + cancelValue + "</span></div></div></section>";
				} else if (cancelValue) {
					dialogHtml[1] = "<div class=\"dialog-btn\"><span class=\"dialog-btn-cancel\">" + cancelValue + "</span></div></div></section>";
				} else if (confirmValue) {
					dialogHtml[1] = "<div class=\"dialog-btn\"><span class=\"dialog-btn-confirm\">" + confirmValue + "</span></div></div></section>";
				} else {
					dialogHtml[1] = "<div class=\"dialog-btn\"><span class=\"dialog-btn-cancel\">取消</span><span class=\"dialog-btn-confirm\">确定</span></div></div></section>";
				}
			}

			if (!hasMask) {
				dialogHtml[2] = "";
			} else {
				dialogHtml[2] = "<div id=\"dialog-box-mask\"></div>";
			}

			return dialogHtml;
		},

		//渲染弹出框
		render: function(element) {
			var that = this,
				$this = $(element),
				dialogHtml = that.create($this),
				$content = that.parseContent();

			$this.replaceWith(dialogHtml[0] + dialogHtml[1]);

			if (typeof ($content) === "object") {
				$content.appendTo(".dialog-box-content");
			} else {
				$(".dialog-box-content").append($content);
			}

			//$("#dialog-box").draggable({
			//	addClasses: false,
			//	cursor: "move",
			//	opacity: 0.5,
			//	snap: false,
			//	delay: 0
			//});

			$("body").append(dialogHtml[2]);
		},

		//解析并处理弹出框内容
		parseContent: function() {
			var that = this,
				content = that.settings.content,
				width = that.settings.width,
				height = that.settings.height,
				type = that.settings.type,
				$iframe = $("<iframe>"),
				random = "?tmp=" + Math.random(),
				urlReg = /^(https?:\/\/|\/|\.\/|\.\.\/)/;

			if (urlReg.test(content)) {

				$iframe.attr({
						src: content + random,
						frameborder: "no",
						scrolling: "no",
						name: "dialog-box-iframe",
						id: "dialog-box-iframe"
					})
					.on("load", function() {

						//动态自适应iframe高度;
						var $iframeBody = $(window.frames["dialog-box-iframe"].document.body),
							iframeWidth = $iframeBody.outerWidth(),
							iframeHeight = $iframeBody.outerHeight(),
							$dialogBox = $("#dialog-box"),
							$content = $(".dialog-box-content"),
							$container = $(".dialog-box-container");

						dialogBoxWidth = iframeWidth + 30;
						dialogBoxHeight = iframeHeight + 110;

						if (that.settings.autoSize) {
							$(this).width(iframeWidth);
							$(this).height(iframeHeight);

							$iframeBody.css({
								margin: "0",
								padding: "0"
							});

							$dialogBox.css({
								width: dialogBoxWidth,
								height: dialogBoxHeight,
								'margin-top': function() {
									if (type === "" || type === "normal") {
										return -Math.round(dialogBoxHeight / 2) + "px";
									} else if (type === "error" || type === "correct") {
										dialogBoxHeight = dialogBoxHeight + 4;
										return -Math.round(dialogBoxHeight / 2) + "px";
									}
								},
								'margin-left': -Math.round(dialogBoxWidth / 2) + "px"
							});

							$content.css({
								//width: iframeWidth,
								//height: iframeHeight

							});

							$container.css({
								//width: dialogBoxWidth,
								//height: dialogBoxHeight

							});

						} else {
							$(this).width(that.settings.width - 40);
							$(this).height(that.settings.height - 126);
						}
					});
				return $iframe;
			} else {
				return content;
			}
		},

		//显示弹出框
		show: function() {
			$("#dialog-box").css("display", "block");

			setTimeout(function() {
				$("#dialog-box").addClass("show");
				$(".body").addClass("blur");
			}, 50);
			$("#dialog-box-mask").show();
		},

		//隐藏弹出框
		hide: function(element) {
			var $this = $(element),
				$dialogBox = $("#dialog-box"),
				$iframe = $("#dialogBox-box-iframe");

			$dialogBox.removeClass("show");

			setTimeout(function() {
				if ($iframe) {
					$iframe.attr("src", "_blank");
				}

				$dialogBox.replaceWith("<div id=\"" + $this.attr("id") + "\"></div/>");
				$("#dialog-box-mask").remove();
				$(".body").removeClass("blur");
			}, 150);
		},

		//设置弹出框样式
		setStyle: function() {
			var that = this,
				$dialog = $("#dialog-box"),
				$container = $(".dialog-box-container"),
				$content = $(".dialog-box-content"),
				$mask = $("#dialog-box-mask"),
				type = that.settings.type,
				effect = "effect";

			//弹出框外框样式
			$dialog.css({
				width: function() {
					if (that.settings.width) {
						return that.settings.width + "px";
					} else {
						return;
					}
				},
				height: function() {
					if (that.settings.height) {
						return that.settings.height + "px";
					} else {
						return;
					}
				},
				'margin-top': function() {
					var height;
					if (type === "" || type === "normal") {
						height = $(this).height();
					} else if (type === "error" || type === "correct") {
						height = $(this).height() + 4;
					}
					return -Math.round(height / 2) + "px";
				},
				'margin-left': function() {
					var width = $(this).width();
					return -Math.round(width / 2) + "px";
				},
				'z-index': that.settings.zIndex
			});

			//弹出框内层容器样式
			$container.css({
				width: function() {
					if (that.settings.width) {
						return that.settings.width + "px";
					} else {
						return;
					}
				},
				height: function() {
					if (that.settings.height) {
						return that.settings.height + "px";
					} else {
						return;
					}
				},
			});

			//弹出框内容样式
			$content.css({
				width: function() {
					if (that.settings.width) {
						return that.settings.width - 40 + "px";
					} else {
						return;
					}
				},
				height: function() {
					if (that.settings.height) {
						return that.settings.height - 126 + "px";
					} else {
						return;
					}
				}
			});

			//遮罩层样式
			$mask.css({
				height: $(document).height() + "px"
			});


			//判断弹出框类型
			switch (that.settings.type) {
			case "correct":
				$container.addClass("correct");
				break;
			case "error":
				$container.addClass("error");
				break;
			default:
				$container.addClass("normal");;
				break;
			}

			//弹出框多种动画效果
			switch (that.settings.effect) {
			case "newspaper":
				$dialog.addClass(effect + "-newspaper");
				break;
			case "fall":
				$dialog.addClass(effect + "-fall");
				break;
			case "scaled":
				$dialog.addClass(effect + "-scaled");
				break;
			case "flip-horizontal":
				$dialog.addClass(effect + "-flip-horizontal");
				break;
			case "flip-vertical":
				$dialog.addClass(effect + "-flip-vertical");
				break;
			case "sign":
				$dialog.addClass(effect + "-sign");
				break;
			default:
				$dialog.addClass(effect + "-fade");
				break;
			}

		},

		//弹出框触屏器(系列事件)
		trigger: function(element, event) {
			var that = this,
				$this = $(element);

			//$('.dialog-box-close,#dialog-box-mask,.dialog-btn-cancel,.dialog-btn-confirm').on('click', function () {
			$(".dialog-box-close, .dialog-btn-cancel, .dialog-btn-confirm").on("click", function() {
				that.hide($this);
			});

			$(document).keyup(function(event) {
				if (event.keyCode === 27) {
					//that.hide($this);
				}
			});

			if (that.settings.autoHide) {
				setTimeout(function() {
					that.hide($this);
				}, that.settings.time);
			}

			if ($.isFunction(that.settings.confirm)) {
				$(".dialog-btn-confirm").on("click", function() {
					that.settings.confirm();
				});
			}

			if ($.isFunction(that.settings.cancel)) {
				$(".dialog-btn-cancel").on("click", function() {
					that.settings.cancel();
				});
			}

		}

	};

	$.fn[pluginName] = function(options) {
		this.each(function() {
			if (!$.data(this, "plugin_" + pluginName)) {
				$.data(this, "plugin_" + pluginName, new dialogBox(this, options));
			}
		});
		return this;
	};

})(jQuery, window, document);