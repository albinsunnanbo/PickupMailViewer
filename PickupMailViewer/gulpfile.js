/// <binding BeforeBuild='default' />
'use strict';

var gulp = require('gulp');

gulp.task('default', ['templates'], function () {

});

var handlebars = require('gulp-handlebars');
var wrap = require('gulp-wrap');
var declare = require('gulp-declare');
var concat = require('gulp-concat');

var handlebarTemplates = 'templates/*.handlebars';
gulp.task('templates', function () {
    gulp.src(handlebarTemplates)
      .pipe(handlebars())
      .pipe(wrap('Handlebars.template(<%= contents %>)'))
      .pipe(declare({
          namespace: 'mailviewer.handlebars.templates',
          noRedeclare: true, // Avoid duplicate declarations
      }))
      .pipe(gulp.dest('templates/dist'));
});


gulp.watch(handlebarTemplates, ['templates']);