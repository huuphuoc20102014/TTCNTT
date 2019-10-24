//  ====================================================================
//	Theme Name: Alamben - Multi-purpose Bootstrap Template
//	Theme URI: http://themeforest.net/user/responsiveexperts
//	Description: This javascript file is using as a settings file. This file includes the sub scripts for the 
//  javascripts used in this template.
//	Version: 1.0
//	Author: Responsive Experts
//	Author URI: http://themeforest.net/user/responsiveexperts
//	Tags:
//  ====================================================================

//	TABLE OF CONTENTS
//	---------------------------
//	 01. OwlCarousel Settings

//  ====================================================================

	$(document).ready(function() {
 
  	$("#portfolio-slider").owlCarousel({
 
      navigation : true, // Show next and prev buttons
      slideSpeed : 300,
      pagination : true,
      singleItem:true
      });
    
      var owl = $("#testimonial-slider");
     
      owl.owlCarousel({
         
          itemsCustom : [
            [0, 1],
            [480,1],
            [600, 2],
            [700, 2],
            [1000, 2],
            [1200, 2],
            [1400, 2],
            [1600, 2]
          ],
          navigation : true,
		  autoPlay : true,
        stopOnHover : true
     
      });
     
    });




