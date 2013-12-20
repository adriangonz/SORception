module.service( 'Auth', [ '$rootScope', '$http', '$location', function( $rootScope, $http, $location ) {
   var service = {
	    SessionID : undefined,
	 

	    login: function (user) {
	        $http({ method: 'POST', url: '/token', data: "grant_type=password&username="+user.username+"&password="+user.password}).
			  success(function(data, status) {
	       		service.SessionID=data;
	       		$location.path("/config");
			  }).
			  error(function(data, status) {
			    console.log(status + " | " + data);
	       		$location.path("/login");
			  }); 	
	    },

	    logout: function(){
	     	  $http({method: 'POST', url: '/api/account/logout'}).
	          success(function(data, status, headers, config) {
	            service.SessionID = undefined;
	       		$location.path("/login");
	          }).
	          error(function(data, status, headers, config) {
	              console.log(data);
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

