module.service('Taller', ['$rootScope', '$http', '$timeout', function ($rootScope, $http, $timeout) {
    var service = {
        orders: [],
        pedidos: [],
        tmp_order: { "data": [] },
        actual_order: undefined,
        actual_pedido: undefined,
        pedido: { "lineas": [], "solicitud": 0 },
        criterios: [
        { name: 'Seleccion Manual', code: '0' },
        { name: 'El primero en llegar', code: '1' },
        { name: 'El mas barato', code: '2' },
        { name: 'El mas nuevo', code: '3' }
            ],

        addLine: function (line) {
            line.update = 'NEW';
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

        removeOrder: function (id) {
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
            service.tmp_order.data = service.actual_order.lines;
            for (var i = 0; i < service.tmp_order.data.length; i++) {
                service.tmp_order.data[i].update = "UPDATED";
                service.tmp_order.data[i].criterio = service.criterios[parseInt(service.tmp_order.data[i].criterio)];
            }
            $rootScope.$broadcast('tmp_order.update');
        },

        getOffersOf: function (id) {
            $http({ method: 'GET', url: '/api/oferta/' + id }).
            success(function (data, status, headers, config) {
                service.offers = data;
                console.log(data);
                $rootScope.$broadcast('offers.update');
            }).
            error(function (data, status, headers, config) {
                alert("getOffers "+status + " | " + data);
            });
        },

        setSolicitudPedido: function (id) {
            service.pedido.solicitud = id;
            $rootScope.$broadcast('pedido.update');
        },


        addLineaPedido: function (line) {
            //Comprobamos si ya existe y la borramos
            service.removeLineaPedido(line.id_linea_oferta);
            //a�ado la nueva linea de pedido
            service.pedido.lineas.push(line);
            $rootScope.$broadcast('pedido.update');
        },

        removeLineaPedido: function (id) {
            //mapeo el array de Lineas por el campo ID y busco la que coincide con el ID de la nueva.
            //var index = service.pedido.lineas.map(function (e) { return e['id_linea_oferta']; }).indexOf(line.id_linea_oferta);
            var lines = $.grep(service.pedido.lineas, function (e) { return e.id_linea_oferta == id? e : null });
            //borro la linea de pedido antigua
            //service.pedido.lineas.splice(index, 1);
            if (lines.length > 0) {
                service.pedido.lineas.splice(service.pedido.lineas.indexOf(lines[0]), 1);
            }
            $rootScope.$broadcast('pedido.update');
        },

        postPedido: function () {
            $http({ method: 'POST', url: '/api/pedido', data: service.pedido }).
             success(function (data, status, headers, config) {
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
            service.pedido = { "lineas": [], "solicitud": 0 };
        },

        getPedidos: function () {
            $http({ method: 'GET', url: '/api/pedido' }).
              success(function (data, status, headers, config) {
                  service.pedidos = data;
                  console.log(data);
                  $rootScope.$broadcast('pedidos.update');
              }).
              error(function (data, status, headers, config) {
                  alert(status + " | " + data);
              });
        },

        getPedido: function (id) {
            $http({ method: 'GET', url: '/api/pedido/' + id }).
                success(function (data, status, headers, config) {
                    service.actual_pedido = data;
                    console.log(data);
                    $rootScope.$broadcast('actual_pedido.update');
                }).
                error(function (data, status, headers, config) {
                    alert(status + " | " + data);
                });
        },

    }

    return service;
}]);