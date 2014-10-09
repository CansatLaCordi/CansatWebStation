$(".flexm").flexigrid({
    url: '/Data/Flexigrid',
    dataType: 'json',
    colModel: [{
        display: 'Id',
        name: 'DataId',
        width: 90,
        sortable: true,
        align: 'center'
    },
    {
        display: 'FlightId',
        name: 'FlightId',
        width: 90,
        sortable: true,
        align: 'center'
    },
    {
        display: 'Datetime',
        name: 'Datetime',
        width: 90,
        sortable: true,
        align: 'center',
        process: FlexiFormatDate
    }, {
        display: 'Temperature',
        name: 'Temperature',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Latitude',
        name: 'Latitude',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Longitude',
        name: 'Longitude',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Altitude',
        name: 'Altitude',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Humidity',
        name: 'Humidity',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Presure',
        name: 'Presure',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'Voltage',
        name: 'Voltage',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'CO',
        name: 'CO',
        width: 90,
        sortable: true,
        align: 'center'
    }, {
        display: 'InternalTemperature',
        name: 'InternalTemperature',
        width: 90,
        sortable: true,
        align: 'center'
    } ],
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
    }],
    sortname: "iso",
    sortorder: "asc",
    usepager: true,
    title: 'Data',
    useRp: true,
    rp: 15,
    showTableToggleBtn: true,
    width: 1024,
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