module.service( 'Auth', [ '$rootScope', '$http', '$location', function( $rootScope, $http, $location ) {
   var service = {
	    SessionID : undefined,
	 
	    login: function () {
	        $http({method: 'GET', url: '/jScrap/api/login'}).
			  success(function(data, status, headers, config) {
	       		service.SessionID=data;
	       		$location.path("/main");
			  }).
			  error(function(data, status, headers, config) {
			  	alert(status+" | "+data);
	       		$location.path("/login");
			  });    	
	    },

	    logout: function(){
	     	  $http({method: 'POST', url: '/jScrap/api/logout'}).
	          success(function(data, status, headers, config) {
	            service.SessionID = undefined;
	       		$location.path("/login");
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },

	    isLoggedIn: function () {
            if(service.SessionID) {
                return true;
            }
	       	$location.path("/login");
        },
   	}
 
   return service;
}]);

