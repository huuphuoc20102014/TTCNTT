/**
 * Particleground demo
 * @author Jonathan Nicol - @mrjnicol
 */

$(document).ready(function() {
  $('#particles').particleground({
    dotColor: '#146f88',
    lineColor: '#0d5d74'
  });
  $('.intro').css({
    'margin-top': -($('.intro').height() / 2)
  });
});