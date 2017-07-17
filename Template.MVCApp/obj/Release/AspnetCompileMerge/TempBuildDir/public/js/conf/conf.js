//#region Opciones Toastr.
var toastr;
if (toastr && toastr.options) {
	toastr.options = {
		closeButton: false,
		debug: false,
		newestOnTop: true,
		progressBar: true,
		positionClass: "toast-top-right",
		preventDuplicates: false,
		onclick: null,
		showDuration: 300,
		hideDuration: 300,
		timeOut: 8000,
		extendedTimeOut: 300,
		showEasing: "linear",
		hideEasing: "linear",
		showMethod: "fadeIn",
		hideMethod: "fadeOut"
	};
}
//#endregion

//#region Moment.
var moment;
if (moment) {
	//Configuración de Date Range.
	var Hoy = new Date();
	var FechaMinima = moment(Hoy).subtract(3, "month");
	var FechaMaxima = moment(Hoy);
	var Diferencia = (moment(FechaMaxima).diff(moment(FechaMinima), "month"));

	var OpcionesDateRangePicker = {
		startDate: moment(Hoy),
		endDate: moment(Hoy),
		//minDate: FechaMinima,
		maxDate: FechaMaxima,
		dateLimit: { months: Diferencia },
		showDropdowns: true,
		showWeekNumbers: false,
		timePicker: false,
		timePickerIncrement: 1,
		timePicker12Hour: false,
		ranges: {
			"Hoy": [Hoy, Hoy],
			"Ayer": [moment(Hoy).subtract(1, "days"), moment(Hoy).subtract(1, "days")],
			"Ayer y Hoy": [moment(Hoy).subtract(1, "days"), moment(Hoy)],
			"Ultimos 7 Dias": [moment(Hoy).subtract(6, "days"), moment(Hoy)],
			"Ultimos 30 Dias": [moment(Hoy).subtract(29, "days"), moment(Hoy)],
			"Este Mes": [moment(Hoy).startOf("month"), moment(Hoy).endOf("month")],
			"El Mes Pasado": [moment(Hoy).subtract(1, "month").startOf("month"), moment(Hoy).subtract(1, "month").endOf("month")],
			//"Ultimos 6 Meses": [moment(Hoy).subtract(6, "month"), moment(Hoy).endOf("month")],
			//"Ultimos 12 Meses": [moment(Hoy).subtract(12, "month"), moment(Hoy).endOf("month")]
		},
		opens: "right",
		drops: "up",
		buttonClasses: "btn btn-small",
		applyClass: "btn-primary",
		cancelClass: "btn-secondary pull-right",
		format: "DD-MM-YYYY",
		separator: " Hasta ",
		autoApply: false,
		locale: {
			applyLabel: "Seleccionar",
			cancelLabel: "Cancelar",
			fromLabel: "Desde",
			toLabel: " Hasta ",
			customRangeLabel: "Rango de Fechas",
			daysOfWeek: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "S&aacute;"],
			monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
			firstDay: 1
		}
	};

	var DateRangePickerCallBack = function (Desde, Hasta, Text) {
		$("#fechaIn").val(Desde.format("YYYY-MM-DD"));
		$("#fechaOut").val(Hasta.format("YYYY-MM-DD"));
	};

	moment.lang("es");
	//Fin Configuración de Date Range.
}
//#endregion
