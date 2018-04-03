/// <binding ProjectOpened='watch' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    less = require("gulp-less"),
    sass = require("gulp-sass"); // добавляем модуль sass

var paths = {
    webroot: "./wwwroot/"
};
// регистрируем задачу для конвертации файла scss в css
gulp.task("sass", function () {
    return gulp.src(paths.webroot + 'scss/*.scss')
        .pipe(sass()).on('error', function (err) { gutil.log(err); })
        .pipe(gulp.dest(paths.webroot + '/styles'));
});

gulp.task('watch', ['sass'], function () {
    gulp.watch(paths.webroot + 'scss/*.scss', ['sass']);
});