module.service('Auth', ['$rootScope', '$http', '$location', '$cookies', function ($rootScope, $http, $location, $cookies) {
   var service = { 

	    login: function (user) {
	        $http({ method: 'POST', url: '/token', data: "grant_type=password&username="+user.username+"&password="+user.password}).
			  success(function (data, status) {
			      $cookies["SessionTaller"] = JSON.stringify(data);
			      $http.defaults.headers.common.Authorization = 'Bearer ' + data.access_token;
			    $rootScope.$broadcast('auth.login');
	       		$location.path("/orders");
			  }).
			  error(function(data, status) {
			    console.log(status + " | " + data);
	       		$location.path("/login");
			  }); 	
	    },

	    logout: function () {
	     	  $http({method: 'POST', url: '/api/account/logout'}).
	          success(function(data, status, headers, config) {
	              delete $cookies["SessionTaller"];
	              $rootScope.$broadcast('auth.login');
	       		$location.path("/login");
	          }).
	          error(function(data, status, headers, config) {
	              console.log(data);
	          });
	    },

	    isLoggedIn: function () {
	        $rootScope.$broadcast('auth.login');
	        SessionTaller = $cookies["SessionTaller"];
	        if (SessionTaller) {
	            SessionTaller = JSON.parse(SessionTaller);
	            $http.defaults.headers.common.Authorization = 'Bearer ' + SessionTaller.access_token;
                return true;
            }
	       	$location.path("/login");
	    },

	    getUsername: function () {
	        SessionTaller = $cookies["SessionTaller"];
	        if (SessionTaller) {
	            SessionTaller = JSON.parse(SessionTaller);
	            return SessionTaller.userName;
	        }
	        return "Sin conexion";
	    },

	    isPrivate: function () {
	        SessionTaller = $cookies["SessionTaller"];
	        if (SessionTaller) {
	            SessionTaller = JSON.parse(SessionTaller);
	            if (SessionTaller.userName != "admin")
	            {
	                $location.path("/orders");
	            }
	        }
	    },

	    getToken: function () {
	        SessionTaller = $cookies["SessionTaller"];
	        if (SessionTaller) {
	            SessionTaller = JSON.parse(SessionTaller);
	            return SessionTaller.access_token;
	        }
	        return undefined;
	    },
   	}

   return service;
}]);
