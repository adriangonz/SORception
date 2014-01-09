module.service( 'Auth', [ '$rootScope', '$http', '$location', function( $rootScope, $http, $location ) {
   var service = {
       SessionID: undefined,
	 

	    login: function (user) {
	        $http({ method: 'POST', url: '/token', data: "grant_type=password&username="+user.username+"&password="+user.password}).
			  success(function(data, status) {
			      service.SessionID = data;
			      $http.defaults.headers.common.Authorization = 'Bearer ' + data.access_token;
			   /* module.run(function ($http) {
			        $http.defaults.headers.common.Authorization = 'Bearer ' + data.access_token;
			    });*/
			    $rootScope.$broadcast('auth.login');
	       		$location.path("/config");
			  }).
			  error(function(data, status) {
			    console.log(status + " | " + data);
	       		$location.path("/login");
			  }); 	
	    },

	    logout: function () {
	        alert("Ola ke ase! te deconeta o ke ase?");
	     	  $http({method: 'POST', url: '/api/account/logout'}).
	          success(function(data, status, headers, config) {
	              service.SessionID = undefined;
	              $rootScope.$broadcast('auth.login');
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

	    getUsername: function () {
	        if (service.SessionID) {
	            return service.SessionID.userName;
	        }
	        return "Sin conexion";
	    },
	    getToken: function () {
	        if (service.SessionID) {
	            return service.SessionID.access_token;
	        }
	        return undefined;
	    },
   	}
 
   return service;
}]);

