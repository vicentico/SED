(function($) {
	var panel;
	var panelHeading;
	var panelBody;
	var panelFooter;
	var iconDown;
	var iconUp;
	var iniciaOcultoAttr;
	var iniciaOculto;
	var icon;
	var pbPanel;
	var mbPanelHeading;

	var metodos = {
		init: function(opciones) {
			opciones = opciones || {};
			panel = $(this);
			panelHeading = $(panel).find("div.panel-heading:first");
			panelBody = $(panel).find("div.panel-body:first");
			panelFooter = $(panel).find("div.panel-footer:first");
			iconDown = opciones.iconDown || "icon-chevron-down";
			iconUp = opciones.iconUp || "icon-chevron-up";
			iniciaOcultoAttr = $(panel).data("opened");
			iniciaOculto = iniciaOcultoAttr != undefined ? iniciaOcultoAttr : (opciones.iniciaOculto || true);
			icon = $(panelHeading).find("i:first");
			pbPanel = $(panel).css("padding-bottom");
			mbPanelHeading = $(panelHeading).css("margin-bottom");
			$(panelHeading).attr("title", "Haga clic para mostrar u ocultar.");

			if (!icon[0]) {
				$(panelHeading).append("<i></i>");
				icon = $(panelHeading).find("i:first");
			}

			$(panelHeading).css("cursor", "pointer");

			$("div.panel[data-panel] > div.panel-heading").click(function(e) {
				e.preventDefault();

				if (e.isPropagationStopped()) return;

				e.stopPropagation();

				panel = $(this).parent();
				panelBody = $(panel).find("div.panel-body:first");
				panelFooter = $(panel).find("div.panel-footer:first");

				if ($(panelBody).is(":visible")) {
					$(panelBody).hide();
					$(panelFooter).hide();
					$(icon).removeClass(iconUp);
					$(icon).removeClass(iconDown);
					$(icon).addClass(iconDown);
					$(panel).css("padding-bottom", "0", "!important");
					$(panelHeading).css("margin-bottom", "0", "!important");
				} else {
					$(panelBody).show();
					$(panelFooter).show();
					$(icon).removeClass(iconUp);
					$(icon).removeClass(iconDown);
					$(icon).addClass(iconUp);
					$(panel).css("padding-bottom", pbPanel);
					$(panelHeading).css("margin-bottom", mbPanelHeading);
				}
			});

			if (iniciaOculto) $(this).panelToggle("cerrar");
			else {
				$(icon).removeClass(iconUp);
				$(icon).removeClass(iconDown);
				$(icon).addClass(iconUp);
			}

			$("[title]").tooltip({
				container: "body"
			});
		},
		abrir: function() {
			$(panelBody).show();
			$(panelFooter).show();
			$(icon).removeClass(iconUp);
			$(icon).removeClass(iconDown);
			$(icon).addClass(iconUp);
			$(panel).css("padding-bottom", pbPanel);
			$(panelHeading).css("margin-bottom", mbPanelHeading);
		},
		cerrar: function() {
			$(panelBody).hide();
			$(panelFooter).hide();
			$(icon).removeClass(iconUp);
			$(icon).removeClass(iconDown);
			$(icon).addClass(iconDown);
			$(panel).css("padding-bottom", "0", "!important");
			$(panelHeading).css("margin-bottom", "0", "!important");
		}
	};

	$.fn.panelToggle = function(metodo) {
		if (metodos[metodo]) {
			return metodos[metodo].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof metodo === "object" || !metodo) {
			return metodos.init.apply(this, arguments);
		} else {
			$.error("Funcion " + metodo + " no existe en el plugin jQuery.panelToggle");
		}
	};
})(jQuery);