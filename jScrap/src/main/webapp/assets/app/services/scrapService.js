module.service( 'Scrap', [ '$rootScope','$http', function( $rootScope, $http) {
   var service = {
     orders: [],
     tmp_offer: [],
 
        addLine: function (line) {
            line.update = 'NEW';
            service.tmp_offer.push(line);
            $rootScope.$broadcast('tmp_offer.update');
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
	    }


   }
 
   return service;
}]);

