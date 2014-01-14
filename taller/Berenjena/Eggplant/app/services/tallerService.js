module.service('Taller', ['$rootScope', '$http', function ($rootScope, $http) {
    var service = {
        orders: [],
        tmp_order: { "data": [] },
        actual_order: undefined,

        addLine: function (line) {
            //line.update = 'NEW';
            service.tmp_order.data.push(line);
            $rootScope.$broadcast('tmp_order.update');
        },

        removeLine: function (line) {
            if (line.update == 'NEW') {
                service.tmp_order.data.splice(service.tmp_order.data.indexOf(line), 1);
            } else {
                line.update = "DELETED";
            }
        },

        postOrder: function (order) {
            $http({ method: 'POST', url: '/api/solicitud', data: order }).
             success(function (data, status, headers, config) {
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

        putOrder: function (order) {
            console.log(order);
            $http({ method: 'PUT', url: '/api/solicitud/' + order.id, data: order }).
             success(function (data, status, headers, config) {
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

        removeOrder: function (order) {
            $http({ method: 'DELETE', url: '/api/solicitud/' + id }).
             success(function (data, status, headers, config) {
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

        getOrders: function () {
            $http({ method: 'GET', url: '/api/solicitud' }).
              success(function (data, status, headers, config) {
                  service.orders = data;
                  console.log(data);
                  $rootScope.$broadcast('orders.update');
              }).
              error(function (data, status, headers, config) {
                  alert(status + " | " + data);
              });
        },

        getOrder: function (id) {
            $http({ method: 'GET', url: '/api/solicitud/' + id }).
                success(function (data, status, headers, config) {
                    service.actual_order = data;
                    console.log(data);
                    $rootScope.$broadcast('actual_order.update');
                }).
                error(function (data, status, headers, config) {
                    alert(status + " | " + data);
                });
        },

        loadActualOrder: function () {
            service.tmp_order.id = service.actual_order.id;
            service.tmp_order.data = service.actual_order.lineaSolicitud;
            for (var i = 0; i < service.tmp_order.data.length; i++) {
                service.tmp_order.data[i].update = "UPDATED";
            }
            $rootScope.$broadcast('tmp_order.update');
        },

    }

    return service;
}]);