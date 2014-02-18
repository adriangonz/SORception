module.service('Auth', ['$rootScope', '$http', '$location', '$cookies', function ($rootScope, $http, $location, $cookies) {
   var service = {
	 
	    login: function (user) {	
	        $http({ method: 'POST', url: '/api/user/authenticate', data: "grant_type=password&username=" + user.username + "&password=" + user.password }).
			  success(function(data, status, headers, config) {
			      $cookies["SessionScrap"] = JSON.stringify(data);
			      console.log(data);
			      $http.defaults.headers.common.Authorization = 'Bearer ' + data.access_token;
			    $rootScope.$broadcast('auth.login');
	       		$location.path("/orders");
			  }).
			  error(function (data, status, headers, config) {
			      $('#err-login').html("El usuario o la contraseño son incorrectos");
			  	console.log(status+" | "+data);
	       		$location.path("/login");
			  });    	
	    },

	    logout: function(){
	     	  $http({method: 'POST', url: '/api/logout'}).
	          success(function(data, status, headers, config) {
	              delete $cookies["SessionScrap"];
	             $http.defaults.headers.common.Authorization = ""; 
	              $rootScope.$broadcast('auth.login');
	       		$location.path("/login");
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },

	    isLoggedIn: function () {
	    	$rootScope.$broadcast('auth.login');
	        SessionScrap = $cookies["SessionScrap"];
	        if (SessionScrap) {
	              SessionScrap = JSON.parse(SessionScrap);
	              $http.defaults.headers.common.Authorization = 'Bearer ' + SessionScrap.access_token;
                return true;
            }
	       	$location.path("/login");
	    },


	    getUsername: function () {
	        SessionScrap = $cookies["SessionScrap"];
	        if (SessionScrap) {
	            SessionScrap = JSON.parse(SessionScrap);
	            return SessionScrap.userName;
	        }
	        return "Sin conexion";
	    },


	    isPrivate: function () {
	        SessionScrap = $cookies["SessionScrap"];
	        if (SessionScrap) {
	            SessionScrap = JSON.parse(SessionScrap);
	            if (SessionScrap.userName != "admin") {
	                $location.path("/orders");
	            }
	        }
	    },

	    getToken: function () {
	        SessionScrap = $cookies["SessionScrap"];
	        if (SessionScrap) {
	            SessionScrap = JSON.parse(SessionScrap);
	            return SessionScrap.access_token;
	        }
	        return undefined;
	    },
   	}
 
   return service;
}]);

