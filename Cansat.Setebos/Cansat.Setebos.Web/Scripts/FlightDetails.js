;

(function (window,$,undefined) {
    var page = {
        lastDataId: 0,
        state: false,
        $btnstart : $("#btnstart"),
        $flightId: $("#FlightId"),
        locations: [],
        urls:{
            getStoreData: '/Flights/GetFlightData/',
            getLastData: '/Flights/LastFlightData/'
        },
        init: function () {
            page.bindings();
            page.getMap();
            page.start();
        },
        bindings: function () {
            page.$btnstart.on("click", function (e) {
                console.log("presed",page.state);
                if (page.state == false)
                    page.start();
                else
                    page.stop();
            })
        },
        getMap: function () {
            page.map = new Microsoft.Maps.Map(document.getElementById('mapContainer'), { showDashboard: false });
        },
        start: function () {
            if (page.state == false) {
                page.state = true;
                page.lastDataId = 0;
                page.getStoreData().done(function () {
                    page.timeObj = setInterval(page.updateMap, 5000);
                }).fail(function (error) {
                    console.log("Error consiguiendo los datos del vuelo");
                });
            }
        },
        stop: function () {
            page.state = false;
            clearInterval(page.timeObj);
        },
        getStoreData:function () {
            return $.ajax({
                url: page.urls.getStoreData ,
                type: 'post',
                dataType: 'json',
                data: { id: page.$flightId.val() }
            }).done(function (data) {
                page.locations = [];
                page.addData(data);
            });
        },
        updateMap: function () {
            $.ajax({
                url: page.urls.getLastData,
                type: 'post',
                dataType: 'json',
                data: { id: page.$flightId.val() , LastDataId: page.lastDataId }
            }).done(function (data) {
                page.addData(data);
            });
        },
        addData: function (data) {
            console.log(data);
            //debugger;
            if(data.length > 0)
                page.lastDataId = data[data.length - 1].Id
            page.map.entities.clear();
            
            for (var i in data) {
                var item = data[i];
                //page.map.entities.clear();
                var location = new Microsoft.Maps.Location(item.Latitude, item.Longitude);
                page.locations.push(location);
                page.setProgressBars(item);
            }
            var polyline = new Microsoft.Maps.Polyline(page.locations, null);
            page.map.entities.push(polyline);
            page.map.setView({ zoom: 10, center: page.locations[page.locations.length -1] })
        },
        setProgressBars: function (item) {
            //console.log(item);
            for (var i in item) {
                var bar = $("#bar" + i).css("width", item[i] /parseFloat( $("#bar" + i).attr("data-max")) * 100);                
            }
        }
    };
    $(document).on("ready", page.init);
})(window,jQuery);