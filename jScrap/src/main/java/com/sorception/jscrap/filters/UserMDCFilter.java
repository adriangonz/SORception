package com.sorception.jscrap.filters;

import java.io.IOException;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.slf4j.MDC;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.filter.OncePerRequestFilter;

public class UserMDCFilter extends OncePerRequestFilter {
	@Override
	protected void doFilterInternal(HttpServletRequest request,
			HttpServletResponse response, FilterChain filterChain)
			throws ServletException, IOException {
		Authentication authentication =
				  SecurityContextHolder.getContext().getAuthentication();
		  if (authentication != null) {
			  MDC.put("userName", authentication.getName());
		  } else {
			  MDC.put("userName", "anonymous");
		  }
		  try {
			  filterChain.doFilter(request, response);
		  } finally {
			  MDC.remove("userName");
		  }
	}
	 
}
