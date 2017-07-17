"use strict";

//#region Variables Globales.
var App = {}, Loader, Body, DialogBox, modal, TemaOscuro;
//#endregion

//#region Punto de Entrada.
$(function () {
	Body = $("body");
	Loader = $("div#loader");
	DialogBox = $("#dialog-box-mask");
    var expandir;

	//#region Verificar Theme.
	if (jQuery().cookie) {
		TemaOscuro = $.cookie("TemaOscuro");
		TemaOscuro = TemaOscuro && TemaOscuro === "true" ? true : false;
	}
	//#endregion

	modal = $("#main-modal")
		.modal({
			show: false,
			keyboard: true
		})
		.on("hidden", function(e) {
			e.preventDefault();
			e.stopPropagation();
			$(this).html("");
		})
        .on("shown", function (e) {
            e.preventDefault();
            e.stopPropagation();
			App.ConfigElements();
            $(this).find("input:visible:not([readonly], [disabled]):first").focus();
	    });

	//$(Loader, DialogBox).on("show", function (e) {
	//	$("div.body").addClass("blur");
	//});

	//$(Loader, DialogBox).on("hide", function (e) {
	//	$("div.body").removeClass("blur");
	//});

	//#region Configuración de Loader.
	$(window).load(function () {
	    setTimeout(function () {
	        $(Loader).hide();
	    }, 0.5 * 1000); //2 Segundos.
	});

	$(window).unload(function () {
		$(Loader).show();
	});

	$(document).on("click", "a.load", function () {
	    $(Loader).show();

	    setTimeout(function () {
	        $(Loader).hide();
	    }, 5 * 1000); //5 Segundos.
	});

	$(document).on("click", "a.disabled", function (e) {
		e.preventDefault();
		return;
	});

	$(document).on("submit", "form", function () {
		$(Loader).show();
	});
	//#endregion

	//#region Configuración Ajax.
	$.ajaxSetup({
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");

			return xhr;
		},
		complete: function () {
			$(Loader).hide();
		},
		cache: false
	});

	$(document).ajaxError(function (xhr, error) {
	    if (error.status === 600) {
	        App.ShowMessage("Usted no tiene los <strong>Privilegios</strong> suficientes para Acceder a la siguiente Url: <strong>" + xhr.target.URL + "</strong>", MessageType.Error);
		}

		if (error.status === 601) {
		    App.ShowMessage("Su sesión ha expirado, será redirigido al formulario de inicio de sesión.", MessageType.Warning);
		    setInterval("location = '';", toastr.options.timeOut);
		}
	});
	//#endregion

	App.ConfigElements();

	//#region Configuración Menú.
	if (jQuery.cookie) {
	    expandir = $.cookie("MenuExpandido");

        expandir = expandir && expandir === "true" ? true : false;

        if (expandir) $(".wrapper").addClass("opened");
        else $(".wrapper").removeClass("opened");
	}

	$("a#pin").click(function(e) {
		e.preventDefault();

		expandir = $(".wrapper").hasClass("opened") ? false : true;

		$.removeCookie("MenuExpandido");
		$.cookie("MenuExpandido", expandir, { expires: 365 });

		if (expandir) $(".wrapper").addClass("opened");
		else $(".wrapper").removeClass("opened");
	});

	var menus = $(".sidebar-wrapper ul.nav");
	
	$.each(menus.not(":first"), function (indice, item) {
	    var menuActivo = $(this).find("li.active");

	    if (menuActivo.length === 0) $(item).toggle("hide");
	    else {
	        var parentMenu = $(menuActivo).parents(".submenu").first();
	        var submenuIcon = $(parentMenu).find("i.icon-chevron-up, i.icon-chevron-down");

	        $(submenuIcon).removeClass("icon-chevron-down");
	        $(submenuIcon).addClass("icon-chevron-up");
	    }
	});

	$(menus).find("a").click(function () {
	    //e.preventDefault();

	    var submenu = $(this).parent().find("ul.nav:first");
	    var submenuIcon = $(this).find("i.icon-chevron-up, i.icon-chevron-down");

	    $(submenu).toggle();

	    if ($(submenu).is(":visible")) {
	        $(submenuIcon).removeClass("icon-chevron-down");
	        $(submenuIcon).addClass("icon-chevron-up");
	    } else {
	        $(submenuIcon).removeClass("icon-chevron-up");
	        $(submenuIcon).addClass("icon-chevron-down");
	    }
	});
	//#endregion
});
//#endregion.

//#region Desencadenadores y Configuración de Elementos.
App.ConfigElements = function () {
	$(document).on("click", "[data-mode='modal']", function (e) {
		e.preventDefault();
		var urlContent = $(this).data("url");
		var modalSize = $(this).data("modal-size");

		if (urlContent === "" || urlContent === "#" || urlContent === "javascript:void(0)") return;

		if (modalSize && modalSize === "large") $(modal).addClass("modal-lg");
		if (modalSize && modalSize === "small") $(modal).removeClass("modal-lg");

		$(modal).load(urlContent, function(response, status) {
			var esError404Controlado = response.search("La página que busca no existe."); //esLogin = response.search("/Access");

			//if (esLogin > 0) {
			//	App.ShowMessage("Su sesión ha expirado, será redirigido al formulario de inicio de sesión dentro de {0} segundos.".format(toastr.options.timeOut / 1000), MessageType.Warning);
			//	setInterval("location = '';", toastr.options.timeOut);
			//}

			if (esError404Controlado <= 0 && status === "success") $(modal).modal("show");
			else $(modal).html("");
		});

		$(modal).on("hide", function () {
			$("div.body").removeClass("blur");
			$(modal).html("");
			$(this).removeClass("modal-lg");
		});
	});

	$(document).find("[title], [data-original-title]").tooltip({ container: "body" });

	$(document).find("input[type='text'], input[type='password'], input[type='email']").focus(function (e) {
		e.preventDefault();

		$(this).select();
	});

	if (jQuery().datepicker) {
		$("[data-datepicker]").datepicker({
			format: "dd-mm-yyyy",
			weekStart: 1,
			language: "es",
			autoclose: true,
			todayHighlight: true,
			clearBtn: true,
			keyboardNavigation: false
		});
	}

	if (jQuery().selectpicker) $(document).find("[data-selectpicker]").selectpicker();

	if (jQuery().panelToggle) $(document).find(".panel[data-panel]").panelToggle();

	if (jQuery().daterangepicker) $(document).find("[data-daterange]").daterangepicker(OpcionesDateRangePicker, DateRangePickerCallBack);

	if (jQuery().bootgrid) {
		$(document).find("[data-bootgrid]").bootgrid({
			rowSelect: true,
			keepSelection: true,
		    caseSensitive: false
		})
		.on("loaded.rs.jquery.bootgrid", function () {
			$(document).find("[title], [data-original-title]").tooltip({ container: "body" });
		});
	}

	if (jQuery().tagsinput) $(document).find("[data-tags]").tagsinput({
	    trimValue: true,
	    confirmKeys: [13, 44], //Enter y Coma
	    allowDuplicates: false
	});

	if (window.prettyPrint) prettyPrint();

	$(document).find("[data-selectable]").dblclick(function (e) {
		e.preventDefault();

		$(this).attr("contenteditable", true);
		
		document.execCommand("selectAll", false, null);
	}).blur(function (e) {
		e.preventDefault();

		$(this).attr("contenteditable", false);
	});

	$(document).find("[data-dragable]").drags();

	var botones = $(".btn, #pin, div.ranges > ul > li, .sidebar-wrapper ul.nav > li > a, .box-icon");

	$.each(botones, function(i, j) {
		if (!$(j).hasClass("waves-effect") && !$(j).hasClass("waves-light")) {
			$(j).addClass("waves-effect");
			$(j).addClass("waves-light");
		}
	});
}
//#endregion

//#region Mensaje tipo Toaster.
var MessageType = {
    Primary: 0,
    Secondary: 1,
    Information: 2,
	Success: 3,
	Warning: 4,
	Error: 5 
};

App.ShowMessage = function (Message, Type_, Title) {
    Type_ = Type_ || MessageType.Info;
    var Title_ = Title || "¡Atención!";
	
	switch (Type_) {
		case MessageType.Info:
			toastr.info(Message, Title_);
			break;
		case MessageType.Success:
			toastr.success(Message, Title_);
			break;
		case MessageType.Warning:
			toastr.warning(Message, Title_);
			break;
		case MessageType.Error:
			toastr.error(Message, Title_);
			break;
		default:
			toastr.info(Message, Title_);
			break;
	}

	$(Loader).hide();
}
//#endregion

//#region Mensaje Tipo Confirm.
var ConfirmEffect = {
	Fade: 1,
	Newspaper: 2,
	Fall: 3,
	Scaled: 4,
	FlipHorizontal: 5,
	FlipVertical: 6,
	Sign: 7
};

var ConfirmType = {
	Info: 1,
	Success: 2,
	Error: 3
};

App.ShowConfirm = function (Options) {
	var Effect_ = Options.Effect || ConfirmEffect.Sign;

	var Efecto = "sign";

	switch (Effect_) {
		case ConfirmEffect.Fade:
			Efecto = "fade";
			break;
		case ConfirmEffect.Newspaper:
			Efecto = "newspaper";
			break;
		case ConfirmEffect.Fall:
			Efecto = "fall";
			break;
		case ConfirmEffect.Scaled:
			Efecto = "scaled";
			break;
		case ConfirmEffect.FlipHorizontal:
			Efecto = "fliphorizontal";
			break;
		case ConfirmEffect.FlipVertical:
			Efecto = "flipvertical";
			break;
		case ConfirmEffect.Sign:
			Efecto = "sign";
			break;
	}

	if (Options) {
		var OptionsDialogBox = {
			autoSize: true,
			autoHide: Options.AutoHide ? Options.AutoHide : false,
			time: Options.AutoHide && Options.AutoHide === true ? 3000 : 0,
			hasMask: true,
			hasClose: false,
			hasBtn: true,
			confirmValue: Options.JustConfirm && Options.JustConfirm === true ? "Aceptar" : "Si",
			confirm: Options.Callback,
			cancelValue: Options.JustConfirm && Options.JustConfirm === true ? null : "No",
			effect: Efecto,
			type: "normal",
			title: Options.Title || "¡Atención!",
			content: Options.Content
		};

		$("#DialogConfirm").dialogBox(OptionsDialogBox);
		$("#dialog-box").drags();
	}
};
//#endregion

//#region Limpiar Formulario.
App.ClearForm = function (form) {
	var $elements = $(form).find("input[type='text'], input[type='password'], input[type='checkbox'], input[type='email'], select");

	$.each($elements, function (indice, item) {
		var tag = item.tagName.toString().toLowerCase();

		switch (tag) {
			case "input":
				var tipo = $(item).attr("type");

				switch (tipo) {
					case "text":
					case "password":
					case "email":
						$(item).val("");
						break;
					case "checkbox":
						$(item).attr("checked", false);
						break;
				}

				if (indice === 0) $(item).focus();
				break;
			case "select":
				$(item).val("");

				if ($(item).is("[data-selectpicker]")) {
					$(item).selectpicker("val", "-1");
				}
				break;
		}
	});
};
//#endregion

//#region Validar Formulario.
App.ValidateForm = function (form) {
    var inputs = $(form).find("input, select");
    var result = true;

	$(inputs).removeClass("error");

	$.each(inputs, function (index, item) {
        var bs = $(item).siblings("button[data-id='" + item.id + "']");

        $(bs).removeClass("error");
    });

	$.each(inputs, function(indice, item) {
		var required = $(item).attr("required");

		if (!required) return true;

		var tag = $(item).prop("tagName").toLowerCase();
		var mensaje = $(item).data("req-message") || $(item).data("message");
		var tipo = $(item).attr("type");
		var valor = $(item).val();

	    if (tag === "input") {
	        switch (tipo) {
	            case "text":
	            case "password":
	            case "email":
	                if (valor.toString().trim().length === 0) {
	                    App.ShowMessage(mensaje, MessageType.Error);
	                    $(item).focus();
	                    $(item).addClass("error");
	                    
	                    result = false;

	                    return false;
	                }

	                var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

	                if (tipo === "email" && !regexEmail.test(valor)) {
	                    App.ShowMessage("Dirección de correo electrónico no es válida.", MessageType.Error);
	                    $(item).focus();
	                    $(item).addClass("error");

	                    result = false;

	                    return false;
	                };

	                break;
	        }
	    }

	    if (tag === "select" || tag === "select-one" || tag === "select-multiple") {
	        valor = $(item).find("option:selected").val();
	        if (valor.toString().trim().length === 0) {
				App.ShowMessage(mensaje, MessageType.Error);
				$(item).focus();
	            var bs = $(item).siblings("button[data-id='" + item.id + "']");
                
	            $(item).addClass("error");
	            $(bs).addClass("error");
				
				result = false;

				return false;
			}
		}

		if (tag === "textarea") {
			if (valor.toString().trim().length === 0) {
				App.ShowMessage(mensaje, MessageType.Error);
				$(item).focus();

				$(item).addClass("error");
				
				result = false;

				return false;
			}
		}
	    return true;
	});

	return result;
};
//#endregion

//#region Validación de Correo Electrónico.
App.ValidateEmail = function (email) {
    var regex_ = /^\w+([-.]\w+)*@\w+([-.]\w+)*\.\w{2,4}$/;

    return regex_.test(email);
}
//#endregion

//#region Llamada Ajax.
App.AjaxCall = function (Opciones) {
    Opciones = Opciones || {};
    var Parametros = "{}";

	if (Opciones.Url == undefined || Opciones.Url === "") {
		console.error("No se especificó la el parámetro [Url] de la llamada.");
		return;
	}

    if (Opciones.ConvertirParametros && Opciones.ConvertirParametros === true) Parametros = JSON.stringify(Opciones.ParametrosAjax);

	$.ajax({
		type: Opciones.Metodo || "GET",
		url: Opciones.Url || "",
		data: Parametros || "{}",
		datatype: Opciones.TipoDatos || "json",
		contentType: Opciones.TipoContenido || "application/json; charset=utf-8",
		crossDomain: Opciones.CORS && Opciones.CORS === true ? true : false,
		cache: false,
		traditional: Opciones.Tradicional && Opciones.Tradicional === true ? true : false,
		//async: Opciones.Asincrono && Opciones.Asincrono == true ? true: false,
		async: true,
		beforeSend: function () {
			$(Loader).show();
		},
		complete: function () {
			setTimeout(function () {
				$(Loader).hide();
			}, 100);
		},
		success: Opciones.Callback || function (result) {
			console.log(result);
		},
		error: function (excepcion) {
		    console.error(excepcion);
		    $(Loader).hide();
		}
	});
};
//#endregion

//#region Establecer Marca para Menú Activo.
App.SetActiveMenu = function (ActiveMenu) {
	var MenuItems = $("body").find("div#menu > ul > li > a");

	$.each(MenuItems, function (Indice, Item) {
		if ($(Item).hasClass(ActiveMenu)) $(Item).parent().addClass("active");
		else $(Item).removeClass("active");
	});
}
//#endregion

//#region Base64.
App.Base64Encode = function (Cadena) {
    return $.base64.encode(Cadena);
}

App.Base64Decode = function (Cadena) {
    return $.base64.decode(Cadena);
}

App.JSONBase64Encode = function (objJson) {
	return JSON.stringify({ d: App.Base64Encode(JSON.stringify(objJson)) });
}
//#endregion

//#region Modal JS.
App.ShowModal = function (options) {
	options = options || {};
	var title = options.title || "¡Atención!";
	var content = options.content || "";

	var $modal =
		"<div class='modal hide animated bounceInDown modal-lg' tabindex='-1'> \
			<div class='modal-header'> \
				<h3>" + title + "</h3> \
				<hr /> \
			</div> \
			<div class='modal-body'>" + content + "</div> \
			<div class='modal-footer'> \
				<hr /> \
				<a href='javascript:void(0);' class='btn btn-primary pull-right' data-dismiss='modal'><i class='icon-ok'></i> Aceptar</a> \
			</div> \
		</div>";

	$($modal)
		.modal({
			keyboard: true
		})
		.on("hidden", function(e) {
			e.preventDefault();
			e.stopPropagation();
			$(this).html("");
		});
};
App.ShowModalFromLink = function(link) {
	var urlContent = $(link).data("url");
	var modalSize = $(link).data("modal-size");

	if (urlContent === "" || urlContent === "#" || urlContent === "javascript:void(0)") return;

	if (modalSize && modalSize === "large") $(modal).addClass("modal-lg");
	if (modalSize && modalSize === "small") $(modal).removeClass("modal-lg");

	$(Loader).show();

	$(modal).load(urlContent, function(response, status) {
		$(Loader).hide();
		var esError404Controlado = response.search("La página que busca no existe."); //esLogin = response.search("/Access");

		//if (esLogin > 0) {
		//	App.ShowMessage("Su sesión ha expirado, será redirigido al formulario de inicio de sesión dentro de {0} segundos.".format(toastr.options.timeOut / 1000), MessageType.Warning);
		//	setInterval("location = '';", toastr.options.timeOut);
		//}

		if (esError404Controlado <= 0 && status === "success") $(this).modal("show");
		else $(this).html("");
	});
}

App.ShowModalWithOptions = function (Options) {
    var UrlContent = Options.Url || "";
    var ModalSize = Options.ModalSize || "";

    if (UrlContent === "" || UrlContent === "#" || UrlContent === "javascript:void(0)") return;

    if (ModalSize && ModalSize === "large") $(modal).addClass("modal-lg");
    if (ModalSize && ModalSize === "small") $(modal).removeClass("modal-lg");

    $(Loader).show();

    $(modal).load(UrlContent, function (Response, Status) {
        $(Loader).hide();

        var esError404Controlado = Response.search("La página que busca no existe.");

        if (esError404Controlado <= 0 && Status === "success") $(this).modal("show");
        else $(this).html("");
    });
}
//#endregion

//#region DelayedCall.
App.DelayedCall = function (Call_, Delay_) {
    setTimeout(function () {
        Call_;
    }, Delay_);
}
//#endregion

//#region PrettyJSON.
App.PrettyJSON = {
	replacer: function(match, pIndent, pKey, pVal, pEnd) {
		var key = "<span class=json-key>";
		var val = "<span class=json-value>";
		var str = "<span class=json-string>";
		var r = pIndent || "";
		if (pKey)
			r = r + key + pKey.replace(/[": ]/g, "") + "</span>: ";
		if (pVal)
			r = r + (pVal[0] === "\"" ? str : val) + pVal + "</span>";
		return r + (pEnd || "");
	},
	Draw: function(obj) {
		var jsonLine = /^( *)("[\w]+": )?("[^"]*"|[\w.+-]*)?([,[{])?$/mg;
		return JSON.stringify(obj, null, 4)
		   .replace(/&/g, "&amp;").replace(/\\"/g, "&quot;")
		   .replace(/</g, "&lt;").replace(/>/g, "&gt;")
		   .replace(jsonLine, App.PrettyJSON.replacer);
	}
};
//#endregion

//#region Crear GUID.
App.CreateGUID = function () {
	return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
		var r = Math.random() * 16 | 0, v = c === "x" ? r : (r & 0x3 | 0x8);
		return v.toString(16);
	});
}
//#endregion

//#region ValidarDiferenciaFechas.
App.ValidarDiferenciaFechas = function (FechaDesde, FechaHasta) {
	if (FechaDesde && FechaHasta && FechaDesde.length > 0 && FechaHasta.length > 0) {
		var FechaDesde_ = moment(FechaDesde, "dd-MM-yyyy");
		var FechaHasta_ = moment(FechaHasta, "dd-MM-yyyy");

		var Diferencia = Math.abs(FechaDesde_.diff(FechaHasta_, "months", true));

		return Diferencia > 3;
	}
	else return true;
};
//#endregion

//#region Extensiones Javascript.
if (!String.prototype.format) {
	String.prototype.format = function () {
		var args = arguments;
		return this.replace(/{(\d+)}/g, function (match, number) {
			return typeof args[number] != "undefined" ? args[number] : match;
		});
	};
}

if (!String.prototype.paddingLeft) {
	String.prototype.paddingLeft = function (paddingValue) {
		return String(paddingValue + this).slice( - paddingValue.length);
	};
}

if (!String.prototype.paddingRight) {
	String.prototype.paddingRight = function (paddingValue) {
		return String(paddingValue + this).slice( - paddingValue.length);
	};
}

if (!Date.prototype.format) {
	Date.prototype.format = function (format) {
		var date = this,
			day = date.getDate(),
			month = date.getMonth() + 1,
			year = date.getFullYear(),
			hours = date.getHours(),
			minutes = date.getMinutes(),
			seconds = date.getSeconds();

		if (!format) {
			format = "MM/dd/yyyy";
		}

		format = format.replace("MM", month.toString().replace(/^(\d)$/, "0$1"));

		if (format.indexOf("yyyy") > -1) {
			format = format.replace("yyyy", year.toString());
		} else if (format.indexOf("yy") > -1) {
			format = format.replace("yy", year.toString().substr(2, 2));
		}

		format = format.replace("dd", day.toString().replace(/^(\d)$/, "0$1"));

		if (format.indexOf("t") > -1) {
			if (hours > 11) {
				format = format.replace("t", "pm");
			} else {
				format = format.replace("t", "am");
			}
		}

		if (format.indexOf("HH") > -1) {
			format = format.replace("HH", hours.toString().replace(/^(\d)$/, "0$1"));
		}

		if (format.indexOf("hh") > -1) {
			if (hours > 12) {
				hours -= 12;
			}

			if (hours === 0) {
				hours = 12;
			}
			format = format.replace("hh", hours.toString().replace(/^(\d)$/, "0$1"));
		}

		if (format.indexOf("mm") > -1) {
			format = format.replace("mm", minutes.toString().replace(/^(\d)$/, "0$1"));
		}

		if (format.indexOf("ss") > -1) {
			format = format.replace("ss", seconds.toString().replace(/^(\d)$/, "0$1"));
		}

		return format;
	};
}
//#endregion
