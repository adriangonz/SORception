module.service( 'Scrap', [ '$rootScope','$http', function( $rootScope, $http) {
   var service = {
     orders: [],
     tmp_offer: {"lines": []},
     actual_order: {},
 
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
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
             });
        },

        putOffer: function (offer) {
            console.log(offer);
            $http({ method: 'PUT', url: '/jScrap/api/solicitud/' + offer.id, data: offer }).
             success(function (data, status, headers, config) {
                 console.log("OK: " + status + " | " + data);
             }).
             error(function (data, status, headers, config) {
                 console.log("Error: " + status + " | " + data);
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
	    }


   }
 
   return service;
}]);

