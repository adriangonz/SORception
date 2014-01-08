module.service( 'Taller', [ '$rootScope', function( $rootScope ) {
   var service = {
       orders: [],
       tmp_order: [],

       addLine: function (line) {
           service.tmp_order.push(line);
           $rootScope.$broadcast('tmp_order.update');
       },

       sendOrder: function (order) {
           //SEND
       },
   }
 
   return service;
}]);

