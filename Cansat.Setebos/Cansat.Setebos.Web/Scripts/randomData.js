
; (function (window,$,undefined) {
    var page = {
        state: false,
        lblState: $("#state"),
        btnStart: $("#btnInsert"),
        pnlResult: $("#results"),
        init: function () {
            page.btnStart.on("click", function (e) {
                e.preventDefault();
                if (page.state == true) {
                    page.stop();
                } else {
                    page.start();
                }
            })
        },
        start: function () {
            page.state = true;
            page.lblState.html("Inserting...");
            page.time = setInterval(page.requestInsert, 1000);
        },
        stop:function () {
            page.state = false;
            page.lblState.html("Stoped!");
            clearInterval(page.time);

        },
        requestInsert: function () {
            $.ajax({
                url: '/Data/InsertRandom',
                type: 'post'
            }).done(function (res) {
                var div = $("<div>").html(res);
                page.pnlResult.append(div);
                if(page.pnlResult.find("div").length > 10)
                setTimeout(function () {
                    page.pnlResult.find("div").first().remove();
                }, 1000);
            }).fail(function (err) {
                console.log(err);
            })
        }
    }

    $(document).on("click", function () {
        page.init();
    });
})(window, jQuery);