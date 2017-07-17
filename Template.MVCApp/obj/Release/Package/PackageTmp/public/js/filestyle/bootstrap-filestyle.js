﻿(function (c) {
    var b = function (d, e) {
        this.options = e;
        this.$elementFilestyle = [];
        this.$element = c(d)
    };
    b.prototype = {
        clear: function () {
            this.$element.val("");
            this.$elementFilestyle.find(":text").val("")
        },
        destroy: function () {
            this.$element.removeAttr("style").removeData("filestyle").val("");
            this.$elementFilestyle.remove()
        },
        disabled: function (d) {
            if (d === true) {
                if (!this.options.disabled) {
                    this.$element.attr("disabled", "true");
                    this.$elementFilestyle.find("label").attr("disabled", "true");
                    this.options.disabled = true
                }
            } else {
                if (d === false) {
                    if (this.options.disabled) {
                        this.$element.removeAttr("disabled");
                        this.$elementFilestyle.find("label").removeAttr("disabled");
                        this.options.disabled = false
                    }
                } else {
                    return this.options.disabled
                }
            }
        },
        buttonBefore: function (d) {
            if (d === true) {
                if (!this.options.buttonBefore) {
                    this.options.buttonBefore = true;
                    if (this.options.input) {
                        this.$elementFilestyle.remove();
                        this.constructor();
                        this.pushNameFiles()
                    }
                }
            } else {
                if (d === false) {
                    if (this.options.buttonBefore) {
                        this.options.buttonBefore = false;
                        if (this.options.input) {
                            this.$elementFilestyle.remove();
                            this.constructor();
                            this.pushNameFiles()
                        }
                    }
                } else {
                    return this.options.buttonBefore
                }
            }
        },
        icon: function (d) {
            if (d === true) {
                if (!this.options.icon) {
                    this.options.icon = true;
                    this.$elementFilestyle.find("label").prepend(this.htmlIcon())
                }
            } else {
                if (d === false) {
                    if (this.options.icon) {
                        this.options.icon = false;
                        this.$elementFilestyle.find("i").remove()
                    }
                } else {
                    return this.options.icon
                }
            }
        },
        input: function (d) {
            if (d === true) {
                if (!this.options.input) {
                    this.options.input = true;
                    if (this.options.buttonBefore) {
                        this.$elementFilestyle.append(this.htmlInput())
                    } else {
                        this.$elementFilestyle.prepend(this.htmlInput())
                    }
                    this.$elementFilestyle.find(".quant-files-filestyle").remove();
                    var e = ""
                      , g = [];
                    if (this.$element[0].files === undefined) {
                        g[0] = {
                            name: this.$element[0].value
                        }
                    } else {
                        g = this.$element[0].files
                    }
                    for (var f = 0; f < g.length; f++) {
                        e += g[f].name.split("\\").pop() + ", "
                    }
                    if (e !== "") {
                        this.$elementFilestyle.find(":text").val(e.replace(/\, $/g, ""))
                    }
                }
            } else {
                if (d === false) {
                    if (this.options.input) {
                        this.options.input = false;
                        this.$elementFilestyle.find(":text").remove();
                        var g = [];
                        if (this.$element[0].files === undefined) {
                            g[0] = {
                                name: this.$element[0].value
                            }
                        } else {
                            g = this.$element[0].files
                        }
                        if (g.length > 0) {
                            var h;
                            if (this.options.classButton.search(/btn-inverse|btn-primary|btn-danger|btn-warning|btn-success/i) !== -1) {
                                h = 'style="background-color: #fff !important; color: 000;"'
                            } else {
                                h = 'style="background-color: #000 !important; color: fff;"'
                            }
                            this.$elementFilestyle.find("label").append(" <span " + h + ' class="quant-files-filestyle badge">' + g.length + "</span>")
                        }
                    }
                } else {
                    return this.options.input
                }
            }
        },
        buttonText: function (d) {
            if (d !== undefined) {
                this.options.buttonText = d;
                this.$elementFilestyle.find("label span").html(this.options.buttonText)
            } else {
                return this.options.buttonText
            }
        },
        classButton: function (d) {
            if (d !== undefined) {
                this.options.classButton = d;
                this.$elementFilestyle.find("label").attr({
                    "class": this.options.classButton
                });
                if (this.options.classButton.search(/btn-inverse|btn-primary|btn-danger|btn-warning|btn-success/i) !== -1) {
                    this.$elementFilestyle.find("label i").addClass("icon-white")
                } else {
                    this.$elementFilestyle.find("label i").removeClass("icon-white")
                }
            } else {
                return this.options.classButton
            }
        },
        classIcon: function (d) {
            if (d !== undefined) {
                this.options.classIcon = d;
                if (this.options.classButton.search(/btn-inverse|btn-primary|btn-danger|btn-warning|btn-success/i) !== -1) {
                    this.$elementFilestyle.find("label").find("i").attr({
                        "class": "icon-white " + this.options.classIcon
                    })
                } else {
                    this.$elementFilestyle.find("label").find("i").attr({
                        "class": this.options.classIcon
                    })
                }
            } else {
                return this.options.classIcon
            }
        },
        classInput: function (d) {
            if (d !== undefined) {
                this.options.classInput = d;
                this.$elementFilestyle.find(":text").addClass(this.options.classInput)
            } else {
                return this.options.classInput
            }
        },
        htmlIcon: function () {
            if (this.options.icon) {
                var d = "";
                if (this.options.classButton.search(/btn-inverse|btn-primary|btn-danger|btn-warning|btn-success/i) !== -1) {
                    d = " icon-white "
                }
                return '<i class="' + d + this.options.classIcon + '"></i> '
            } else {
                return ""
            }
        },
        htmlInput: function () {
            if (this.options.input) {
                return '<input type="text" class="' + this.options.classInput + '" disabled> '
            } else {
                return ""
            }
        },
        pushNameFiles: function () {
            var d = ""
              , f = [];
            if (this.$element[0].files === undefined) {
                f[0] = {
                    name: this.$element.value
                }
            } else {
                f = this.$element[0].files
            }
            for (var e = 0; e < f.length; e++) {
                d += f[e].name.split("\\").pop() + ", "
            }
            if (d !== "") {
                this.$elementFilestyle.find(":text").val(d.replace(/\, $/g, ""))
            } else {
                this.$elementFilestyle.find(":text").val("")
            }
        },
        constructor: function () {
            var i = this
              , g = ""
              , h = this.$element.attr("id")
              , d = [];
            if (h === "" || !h) {
                h = "filestyle-" + c(".bootstrap-filestyle").length;
                this.$element.attr({
                    id: h
                })
            }
            if (this.options.buttonBefore) {
                g = '<label for="' + h + '" style="margin-right: 4px;" class="' + this.options.classButton + '" ' + (this.options.disabled ? 'disabled="true"' : "") + ">" + this.htmlIcon() + "<span>" + this.options.buttonText + "</span></label>" + this.htmlInput()
            } else {
                g = this.htmlInput() + '<label for="' + h + '" class="' + this.options.classButton + '" ' + (this.options.disabled ? 'disabled="true"' : "") + "> " + this.htmlIcon() + "<span>" + this.options.buttonText + "</span></label>"
            }
            this.$elementFilestyle = c('<div class="bootstrap-filestyle" style="display: inline-block;">' + g + "</div>");
            var f = this.$elementFilestyle.find("label");
            var e = f.parent();
            e.attr("tabindex", "0").keypress(function (j) {
                if (j.keyCode === 13 || j.charCode === 32) {
                    f.click()
                }
            });
            this.$element.css({
                position: "absolute",
                clip: "rect(0,0,0,0)"
            }).attr("tabindex", "-1").after(this.$elementFilestyle);
            if (this.options.disabled) {
                this.$element.attr("disabled", "true")
            }
            this.$element.change(function () {
                var j = "";
                if (this.files === undefined) {
                    d[0] = {
                        name: this.value
                    }
                } else {
                    d = this.files
                }
                for (var k = 0; k < d.length; k++) {
                    j += d[k].name.split("\\").pop() + ", "
                }
                if (j !== "") {
                    i.$elementFilestyle.find(":text").val(j.replace(/\, $/g, ""))
                } else {
                    i.$elementFilestyle.find(":text").val("")
                }
                if (i.options.input == false) {
                    var l;
                    if (i.options.classButton.search(/btn-inverse|btn-primary|btn-danger|btn-warning|btn-success/i) !== -1) {
                        l = 'style="background-color: #fff !important; color: #000;"'
                    } else {
                        l = 'style="background-color: #000 !important; color: #fff;"'
                    }
                    if (i.$elementFilestyle.find(".quant-files-filestyle").length == 0) {
                        i.$elementFilestyle.find("label").append(" <span " + l + ' class="quant-files-filestyle badge">' + d.length + "</span>")
                    } else {
                        if (d.length == 0) {
                            i.$elementFilestyle.find(".quant-files-filestyle").remove()
                        } else {
                            i.$elementFilestyle.find(".quant-files-filestyle").html(d.length)
                        }
                    }
                } else {
                    i.$elementFilestyle.find(".quant-files-filestyle").remove()
                }
            });
            if (window.navigator.userAgent.search(/firefox/i) > -1) {
                this.$elementFilestyle.find("label").click(function () {
                    i.$element.click();
                    return false
                })
            }
        }
    };
    var a = c.fn.filestyle;
    c.fn.filestyle = function (e, d) {
        var f = ""
          , g = this.each(function () {
              if (c(this).attr("type") === "file") {
                  var j = c(this)
                    , h = j.data("filestyle")
                    , i = c.extend({}, c.fn.filestyle.defaults, e, typeof e === "object" && e);
                  if (!h) {
                      j.data("filestyle", (h = new b(this, i)));
                      h.constructor()
                  }
                  if (typeof e === "string") {
                      f = h[e](d)
                  }
              }
          });
        if (typeof f !== undefined) {
            return f
        } else {
            return g
        }
    }
    ;
    c.fn.filestyle.defaults = {
        buttonText: "Choose file",
        input: true,
        icon: true,
        buttonBefore: false,
        disabled: false,
        classButton: "btn btn-default",
        classInput: "input-large",
        classIcon: "icon-folder-open"
    };
    c.fn.filestyle.noConflict = function () {
        c.fn.filestyle = a;
        return this
    }
    ;
    c(function () {
        c(".filestyle").each(function () {
            var e = c(this)
              , d = {
                  input: e.attr("data-input") === "false" ? false : true,
                  icon: e.attr("data-icon") === "false" ? false : true,
                  buttonBefore: e.attr("data-buttonBefore") === "true" ? true : false,
                  disabled: e.attr("data-disabled") === "true" ? true : false,
                  buttonText: e.attr("data-buttonText"),
                  classButton: e.attr("data-classButton"),
                  classInput: e.attr("data-classInput"),
                  classIcon: e.attr("data-classIcon")
              };
            e.filestyle(d)
        })
    })
})(window.jQuery);
