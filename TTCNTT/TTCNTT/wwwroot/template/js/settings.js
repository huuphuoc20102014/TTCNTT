//  ====================================================================
//	Theme Name: Alamben - Multi-purpose Bootstrap Template
//	Theme URI: http://themeforest.net/user/responsiveexperts
//	Description: This javascript file is using as a settings file. This file includes the sub scripts for the javascripts used in this template.
//	Version: 1.0
//	Author: Responsive Experts
//	Author URI: http://themeforest.net/user/responsiveexperts
//	Tags:
//  ====================================================================

//	TABLE OF CONTENTS
//	---------------------------
//	 01. Preloader
//	 02. Tabs Settings
//	 03. Scroll To Top
//   04. Adding fixed position to header
//	 05. Menu In Mobile
//	 06. Banner Settings (flex-slider)
//	 07. Mini Grid
//	 08. Accordion Settings
//	 09. Progress Bar Settings

//  ====================================================================


	(function() {
		"use strict";
	
	// ----------------  01. Preloader     ----------------------------
	// ----------------------------------------------------------------
	
	$(window).load(function() {
		$("#loader").fadeOut();
		$("#mask").delay(1000).fadeOut("slow");
	// -------------------- 02. Tabs Settings -------------------------
	// ----------------------------------------------------------------
		var tab_content = $('.tab-content');
		var tab_heading = $(".tab-heading")
		tab_content.hide();
		$(".tab-content:first").show();
	
		/* if in tab mode */
		$(".tab-menu li").on('click',function() {
			
		  tab_content.hide();
		  var activeTab = $(this).attr("rel"); 
		  $("#"+activeTab).fadeIn();		
			
		  $(".tab-menu li").removeClass("active");
		  $(this).addClass("active");
	
		  tab_heading.removeClass("current-item");
		  $(".tab-heading[rel^='"+activeTab+"']").addClass("current-item");
		  
		});
		/* if in drawer mode */
		tab_heading.on('click',function() {
		  
		  tab_content.hide();
		  var d_activeTab = $(this).attr("rel"); 
		  $("#"+d_activeTab).fadeIn();
		  
		  tab_heading.removeClass("current-item");
		  $(this).addClass("current-item");
		  
		  $(".tab-menu li").removeClass("active");
		  $(".tab-menu li[rel^='"+d_activeTab+"']").addClass("active");
		});
	
		// ------------Vertical Tab ----------------------------------------------
		var tab_vertical_content = $(".tab-vertical-content");
		tab_vertical_content.hide();
		$(".tab-vertical-content:first").show();
	
		 /* if in tab mode */
		$(".tabs-vertical li").on('click',function() {
			
		  tab_vertical_content.hide();
		  var activeTab = $(this).attr("rel"); 
		  $("#"+activeTab).fadeIn();		
			
		  $(".tabs-vertical li").removeClass("active");
		  $(this).addClass("active");
	
		  $(".tab-drawer-heading").removeClass("active-item");
		  $(".tab-drawer-heading[rel^='"+activeTab+"']").addClass("active-item");
		  
		});
		/* if in drawer mode */
		$(".tab-drawer-heading").on('click',function() {
		  
		  tab_vertical_content.hide();
		  var d_activeTab = $(this).attr("rel"); 
		  $("#"+d_activeTab).fadeIn();
		  
		  $(".tab-drawer-heading").removeClass("active-item");
		  $(this).addClass("active-item");
		  
		  $(".tabs-vertical li").removeClass("active");
		  $(".tabs-vertical li[rel^='"+d_activeTab+"']").addClass("active");
		});
		
	});
	
	// ----------------  03. Scroll To Top   --------------------------
	// ----------------------------------------------------------------
	
	
	
	
	$(function() {
		$('a[href*=#]:not([href=#])').on('click',function(e){
			$('.menu li').removeClass('active');
			$(this).parent().addClass('active');
			if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') || location.hostname == this.hostname		) {
	
				var target = $(this.hash);
				target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
				if (target.length) {
					$('html,body').animate({
						scrollTop: target.offset().top
					}, 1000);
					return false;
				}
			}
			
		});
	});
	
	// ---------   04. Adding fixed position to header   -------------- 
	// ----------------------------------------------------------------
	
	$(document).on('scroll',function() {
		var header_area = $('.header-area');
		if ($(document).scrollTop() >= 1) {
		  header_area.addClass('navbar-fixed-top');
		} else {
		  header_area.removeClass('navbar-fixed-top');
		}
	});
	
	// -------------   05. Menu In Mobile   --------------------------
	// ----------------------------------------------------------------
	
	 $('.mob-icon').on('click',function() {
            $('.menu').slideToggle("slow");
     });
		
	 // Filter menu
		$(".simplefilter li").on('click',function(){
		  if (!$(this).hasClass("active")) {
			$(".simplefilter li.active").removeClass("active");
			$(this).addClass("active");
		  }
		});
   	 
	// -----------  06. Banner Settings (flex-slider) -----------------
	// ----------------------------------------------------------------
	
	 $('.flexslider').flexslider({
        animation: "fade",
		autoplay: "true"
      });
	
	
		
    // -------------------- 07. Mini Grid -----------------------------
	// ----------------------------------------------------------------

	   (function(){
       	 function grid() {
          minigrid({
            container: '.mini-grid-area ',
            item: '.mini-grid-area > div ',
            gutter:3
          });
       	 }

         window.addEventListener('resize', function(){
          	grid();
         });
         grid();
       })
	  (); 
	  
	
	
	// -------------------- 08. Accordion Settings --------------------
	// ----------------------------------------------------------------
	
    //  Accordion Simple 
   
	$(".accordion-simple div").show();
    setTimeout("$('.accordion-simple .panel').slideToggle('slow');", 1000);
    $(".accordion-simple .tab").on('click',function() {
        $(this).next(".panel").slideToggle("slow").siblings(".panel:visible").slideUp("slow");
        $(this).toggleClass("current");
        $(this).siblings(".tab").removeClass("current");
    });
	
	//-------------  Accordion Auto  ---------------------------------
	
	$("#accordion-auto div").first().css('display', 'block');
	
	// Get all the links.
	var link = $("#accordion-auto a");
	
	// On clicking of the links do something.
	link.on('click', function(e) {
    e.preventDefault();
    var a = $(this).attr("href");
    $(a).slideDown('slow');
	
    //$(a).slideToggle('fast');
    $("#accordion-auto div").not(a).slideUp('slow');
	});
	
	// -------------------- 09. Progress Bar Settings --------------------
	// ------------------------------------------------------------------
	
	 
	setTimeout(function(){

		$('.progress .bar').each(function() {
			var me = $(this);
			var perc = me.attr("data-percentage");
	
			//TODO: left and right text handling
	
			var current_perc = 0;
	
			var progress = setInterval(function() {
				if (current_perc>=perc) {
					clearInterval(progress);
				} else {
					current_perc +=1;
					me.css('width', (current_perc)+'%');
				}
	
				me.text((current_perc)+'%');
	
			}, 50);
	
		});
	
	},300);

})(jQuery);


