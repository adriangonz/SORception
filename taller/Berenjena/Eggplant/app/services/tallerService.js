module.service('Taller', ['$rootScope', '$http', function ($rootScope, $http) {
    var service = {
        orders: [],
        tmp_order:  {"data" : []},

       addLine: function (line) {
           service.tmp_order.data.push(line);
           $rootScope.$broadcast('tmp_order.update');
       },

       sendOrder: function (order) {
           $http({ method: 'POST', url: '/api/solicitud', data: order }).
            success(function (data, status, headers, config) {
                console.log("OK: " + status + " | " + data);
                alert("OK: " + status + " | " + data);
            }).
            error(function (data, status, headers, config) {
                console.log("Error: " + status + " | " + data);
            });
       },
   }
 
   return service;
}]);

