; (function (window, $, undefined) {
    var page = {
        table: $("#tblFlights tbody"),
        btnReload: $("#btnReload"),
        init: function () {
            page.btnReload.on("click", page.getActiveFlights);
            page.getActiveFlights();
        },
        getActiveFlights: function (e) {
            $.ajax({
                url: '/flights/ActualFlights',
                type: 'post',
                dataType: 'json'
            }).done(function (res) {
                var trs = $("<tbody>");
                for (var i in res) {
                    var item = res[i];
                    var tr = $("<tr>")
                        .append($("<td>").html(item.Name))
                        .append($("<td>").html(item.Active ? "Activo" : ""))
                        .append($("<td>")
                            .append($("<a>").html("Editar").attr("href", "/Flights/Edit/" + item.FlightId))
                            .append($("<a>").html("Datos").attr("href", "/Data/Flight/" + item.FlightId))
                        );
                    trs.append(tr);
                }
                page.table.html(trs.html());
            }).fail(function (error) {
                alert(error.Message);
                console.log(error);
            });
        }
    }
    $(document).on("ready", function () {
        page.init();
    })
})(window, jQuery);


$(".flexme4").flexigrid({
    url: '/flights/ActualFlights',
    dataType: 'json',
    colModel: [{
        display: 'Flight Id',
        name: 'FlightId',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Activo',
        name: 'Active',
        width: 120,
        sortable: true,
        align: 'left'
    }, {
        display: 'Nombre',
        name: 'Name',
        width: 120,
        sortable: true,
        align: 'left'
    }],
    buttons: [{
        name: 'Add',
        bclass: 'add',
        onpress: Example4
    }
        ,
        {
            name: 'Edit',
            bclass: 'edit',
            onpress: Example4
        }
        ,
        {
            name: 'Delete',
            bclass: 'delete',
            onpress: Example4
        }
        ,
        {
            separator: true
        }
    ],
    searchitems: [{
        display: 'Flight Id',
        name: 'FlightId'
    }, {
        display: 'Nombre',
        name: 'Name',
        isdefault: true
    }],
    sortname: "iso",
    sortorder: "asc",
    usepager: true,
    title: 'Flights',
    useRp: true,
    rp: 15,
    showTableToggleBtn: true,
    width: 750,
    height: 200
});


function Example4(com, grid) {
    if (com == 'Delete') {
        var conf = confirm('Delete ' + $('.trSelected', grid).length + ' items?')
        if (conf) {
            $.each($('.trSelected', grid),
                function (key, value) {
                    $.get('example4.php', { Delete: value.firstChild.innerText }
                        , function () {
                            // when ajax returns (callback), update the grid to refresh the data
                            $(".flexme4").flexReload();
                        });
                });
        }
    }
    else if (com == 'Edit') {
        var conf = confirm('Edit ' + $('.trSelected', grid).length + ' items?')
        if (conf) {
            $.each($('.trSelected', grid),
                function (key, value) {
                    // collect the data
                    var OrgEmpID = value.children[0].innerText; // in case we're changing the key
                    var EmpID = prompt("Please enter the New Employee ID", value.children[0].innerText);
                    var Name = prompt("Please enter the Employee Name", value.children[1].innerText);
                    var PrimaryLanguage = prompt("Please enter the Employee's Primary Language", value.children[2].innerText);
                    var FavoriteColor = prompt("Please enter the Employee's Favorite Color", value.children[3].innerText);
                    var FavoriteAnimal = prompt("Please enter the Employee's Favorite Animal", value.children[4].innerText);

                    // call the ajax to save the data to the session
                    $.get('example4.php',
                        {
                            Edit: true
                            , OrgEmpID: OrgEmpID
                            , EmpID: EmpID
                            , Name: Name
                            , PrimaryLanguage: PrimaryLanguage
                            , FavoriteColor: FavoriteColor
                            , FavoritePet: FavoriteAnimal
                        }
                        , function () {
                            // when ajax returns (callback), update the grid to refresh the data
                            $(".flexme4").flexReload();
                        });
                });
        }
    }
    else if (com == 'Add') {
        // collect the data
        var EmpID = prompt("Please enter the Employee ID", "5");
        var Name = prompt("Please enter the Employee Name", "Mark");
        var PrimaryLanguage = prompt("Please enter the Employee's Primary Language", "php");
        var FavoriteColor = prompt("Please enter the Employee's Favorite Color", "Tan");
        var FavoriteAnimal = prompt("Please enter the Employee's Favorite Animal", "Dog");

        // call the ajax to save the data to the session
        $.get('example4.php', { Add: true, EmpID: EmpID, Name: Name, PrimaryLanguage: PrimaryLanguage, FavoriteColor: FavoriteColor, FavoritePet: FavoriteAnimal }
            , function () {
                // when ajax returns (callback), update the grid to refresh the data
                $(".flexme4").flexReload();
            });
    }
}