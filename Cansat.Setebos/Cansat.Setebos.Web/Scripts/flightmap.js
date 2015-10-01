;


(function (window,$,undefined) {
    window.page = {
        locations: [],
        $flightId: $("#FlightId"),
        init: function () {
            page.getMap();
            page.getData();
        },
        getData:function () {
            $.ajax({
                url: "/Flights/GetLocations/" + page.$flightId.val(),
                type:"post",
                dataType: "json"
            }).done(page.addData);
        },
        getMap: function () {
            page.map = new Microsoft.Maps.Map(document.getElementById('mapContainer'), { showDashboard: false, credentials: 'AiY3dqw6wravTPdlQ7Jp__suONS-ntyfwrJwXuyY1RCPHyEhBrW0ySAabd9-U1gm' });
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
             
            }
            var polyline = new Microsoft.Maps.Polyline(page.locations, null);
            page.map.entities.push(polyline);
            page.map.setView({ zoom: 10, center: page.locations[page.locations.length -1] })
        }
       
    };
    
    $(window.document).on("ready", window.page.init);
})(window,jQuery);