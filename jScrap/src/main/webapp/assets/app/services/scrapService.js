module.service( 'Scrap', [ '$rootScope','$http', function( $rootScope, $http) {
   var service = {
     orders: [],
     offers: [],
     pedidos: [],
     tmp_offer: {"lines": []},
     actual_order: {},
     actual_offer: {},
     actual_pedido: {},
 
        addLine: function (line) {
            line.status = 'NEW';
            service.tmp_offer.lines.push(line);
            $rootScope.$broadcast('tmp_offer.update');
        },

        removeLine: function (line) {
            if (line.status == 'NEW') {
                service.tmp_offer.lines.splice(service.tmp_offer.lines.indexOf(line), 1);
            } else {
                line.status = "DELETED";
            }
            $rootScope.$broadcast('tmp_offer.update');
        },

        postOffer: function (offer) {
            $http({ method: 'POST', url: '/jScrap/api/offer', data: offer }).
             success(function (data, status, headers, config) {
                 console.log(data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

        putOffer: function (offer) {
            console.log(offer);
            var id = offer.id;
            delete offer.id;
            $http({ method: 'PUT', url: '/jScrap/api/offer/' + id , data: offer }).
             success(function (data, status, headers, config) {
                 console.log(data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

        removeOffer: function (id) {
            $http({ method: 'DELETE', url: '/jScrap/api/offer/' + id }).
             success(function (data, status, headers, config) {
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

	    getOffers: function () {
	        $http({ method: 'GET', url: '/jScrap/api/offer' }).
	          success(function (data, status, headers, config) {
	              service.offers = data;
	              console.log(data);
	              $rootScope.$broadcast('offers.update');
	          }).
	          error(function (data, status, headers, config) {
	              alert(status + " | " + data);
	          });
	    },
	    
	    getOrders: function () {
	        $http({ method: 'GET', url: '/jScrap/api/order' }).
	          success(function (data, status, headers, config) {
	              service.orders = data;
	              console.log(data);
	              $rootScope.$broadcast('orders.update');
	          }).
	          error(function (data, status, headers, config) {
	              alert(status + " | " + data);
	          });
	    },
	    
	    getActualOrder: function (id) {
	        $http({ method: 'GET', url: '/jScrap/api/order/'+id }).
	          success(function (data, status, headers, config) {
	              service.actual_order = data;
	              console.log(data);
	              $rootScope.$broadcast('actual_order.update');
	          }).
	          error(function (data, status, headers, config) {
	              alert(status + " | " + data);
	          });
	    },	

	    getActualOffer: function (id) {
	        $http({ method: 'GET', url: '/jScrap/api/offer/'+id }).
	          success(function (data, status, headers, config) {
	              service.actual_offer = data;
	              console.log(data);
	              $rootScope.$broadcast('actual_offer.update');
	              service.loadActualOffer();
	          }).
	          error(function (data, status, headers, config) {
	              alert(status + " | " + data);
	          });
	    },

	    loadActualOffer: function () {
            service.tmp_offer.id = service.actual_offer.id;
            service.tmp_offer.lines = service.actual_offer.lines;
            for (var i = 0; i < service.tmp_offer.lines.length; i++) {
            	delete service.tmp_offer.lines[i].creationDate;
            	delete service.tmp_offer.lines[i].updatedDate;
            	delete service.tmp_offer.lines[i].offerId;
                service.tmp_offer.lines[i].status = "UPDATED";
            }
            $rootScope.$broadcast('tmp_offer.update');
        },

       	getPedidos: function () {
	        $http({ method: 'GET', url: '/jScrap/api/accepted' }).
	          success(function (data, status, headers, config) {
	              service.pedidos = data;
	              console.log(data);
	              $rootScope.$broadcast('pedidos.update');
	          }).
	          error(function (data, status, headers, config) {
	              alert(status + " | " + data);
	          });
	    },

	    getActualPedido: function (id) {
	        $http({ method: 'GET', url: '/jScrap/api/offer/'+id }).
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

