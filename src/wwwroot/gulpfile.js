var
    gulp = require('gulp'),
    rename = require('gulp-rename'),
    $ = require('gulp-load-plugins')({ lazy: true }),
    config = require('./gulp.config')(),
    tsProject = $.typescript.createProject({
        "target": "ES5",
        "noImplicitAny": true,
        "sourceMap": false,
        "noExternalResolve": true
    }),
    runSequence = require('run-sequence');

/**
 * Wire-up the bower dependencies and custom js files
 * @return {Stream}
 */
gulp.task('_js-inject', function() {
    log('Wiring the bower dependencies into the html');

    var wiredep = require('wiredep').stream;
    var options = config.getWiredepDefaultOptions();

    return gulp
        .src(config.index)
        .pipe(wiredep(options))
        .pipe(inject(config.js.appFiles, '', config.js.order))
        .pipe(gulp.dest(config.root));
});

gulp.task('_styles-inject', ['sass'], function() {
    log('Wire up css into the html, after files are ready');

    return gulp
        .src(config.index)
        .pipe(inject(config.styles.css))
        .pipe(gulp.dest(config.root));
});

/**
 * Creates and injects js files into index.html
 */
gulp.task('bundle', function(callback) {
    runSequence('_js-clean', 'ts', '_js-inject', 'sass', '_styles-inject', callback);
});

/**
* Removes all *.js and *.js.map files from ./app
* @return {Stream}
*/
gulp.task('_js-clean', function() {
    return gulp
        .src([config.js.all, config.js.maps])
        .pipe($.clean());
});

/**
*  Processess javascript files and adds dependency injection annotations ($inject) where /* @ngInject */
/* is found or for angular standard injections, ex controllers
*  @return {Stream}
*/
gulp.task('_js-annotate', function() {
    return gulp.src(config.js.appFiles)
        .pipe($.ngAnnotate())
        .pipe(gulp.dest(config.app));
});

/**
 * Lint all typescript files
 * @return {Stream}
 */
gulp.task('_ts-lint', function() {
    return gulp
        .src(config.ts.allTs)
        .pipe($.tslint())
        .pipe($.tslint.report('prose'));
});

/**
 * Compiling Typescript --> Javascript
 * @return {Stream}
 */
gulp.task('_ts-compile', function() {
    var tsResult = gulp
        .src([config.ts.allTs, config.ts.libTypingsAllTs, './typings/main.d.ts'])
        .pipe($.sourcemaps.init())
        .pipe($.typescript(tsProject));

    tsResult.dts.pipe(gulp.dest('.'));

    return tsResult.js.pipe($.sourcemaps.write('.'))
        .pipe(gulp.dest(config.app));
});

/*
 * Inject all ts project files in to _all.d.ts file.
 * @return {Stream}
 */
gulp.task('_ts-ref', function() {
    log("Updating _all.d.ts file.")
    var tsFiles = [config.ts.allTs, '!' + config.ts.allDtsFilePath, '!' + config.ts.specTs];
    return gulp.src('buildTemplates/_all.d.ts')
        .pipe($.inject(gulp.src(tsFiles, { read: false }), {
            starttag: '//<--inject:ts-->',
            endtag: '//<--inject:end:ts-->',
            transform: function(filepath, file, i, length) {
                return '/// <reference path="../..' + filepath + '" />';
            }
        }))
        .pipe(gulp.dest('app/typings/'));
});
//
/**
*  Lints and compiles typescript files
*  @return {Stream}
*/
gulp.task('ts', function(callback) {
    log('Compiling typescript to javascript');
    runSequence('_ts-ref', '_ts-lint', '_ts-compile', '_js-annotate', callback);
});

/**
 * Watching changes in typescript files
 */
gulp.task('ts-watch', function() {
    gulp.watch([config.ts.allTs, '!' + config.ts.allDtsFilePath], ['ts']);
});

/**
 * Inject all the spec files into the specRunner.html
 * @return {Stream}
 */
gulp.task('build-specs', ['_build-specs-templatecache'], function() {
    log('Building the spec runner');

    var wiredep = require('wiredep').stream;
    var options = config.getWiredepDefaultOptions();
    var templateCache = config.build.testInput + config.build.templateCache.file;

    options.devDependencies = true;

    return gulp
        .src('buildTemplates/specRunner.html')
        .pipe(wiredep(options))
        .pipe(inject(config.js.appFilesToTest, '', config.js.order))
        .pipe(inject(config.js.specsAndMocks, 'specs', config.js.specsAndMocksOrder))
        .pipe(inject(templateCache, 'templates'))
        .pipe(gulp.dest(config.root));
});

/**
 * Building everything
 */
gulp.task('build', ['_optimize'], function() {
    log('Building everything');

    log('Cleaning output folder');

    gulp.src([config.build.temp])
        .pipe($.clean());

    log('Copying static files');
    gulp.src(config.build.staticAppFiles)
        .pipe(gulp.dest(config.build.output));

    log('Copying images');
    gulp.src(config.build.imagesFiles)
        .pipe(gulp.dest(config.build.imagesDest));

    log('Copying translation files');
    gulp.src(config.build.staticTranslationsFiles)
        .pipe(gulp.dest(config.build.translationsDest));

    log('Copying fonts');
    gulp.src(config.build.fonts)
        .pipe(gulp.dest(config.build.output + config.bower.directory + 'font-awesome/fonts/'));
    gulp.src(config.build.bootstrapFonts)
        .pipe(gulp.dest(config.build.output + config.bower.directory + 'bootstrap-sass/assets/fonts/bootstrap/'));

    log('Copying web.config file');
    gulp.src(config.build.webConfigFile)
        .pipe(rename(function(path) {
            path.basename = path.basename.replace('_', '');
        }))
        .pipe(gulp.dest(config.build.output));

    log('Deployed to build folder');
});

/**
 * Building everything in local env
 * @return {Stream}
 */
gulp.task('build-local', function(callback) {
    runSequence('bundle', 'build-specs', callback);
});

/**
 * Remove all js and html from the build and temp folders
 * @param  {Function} done - callback when complete
 */
gulp.task('_clean-code', function() {
    var files = [].concat(
        config.build.temp + '**/*.js',
        config.build.output + 'js/**/*.js',
        config.build.output + 'js/**/*.js.map',
        config.build.output + '**/*.html'
    );

    gulp.src(files)
        .pipe($.clean());
});

gulp.task('_build-templatecache', ['_clean-code'], function() {
    log('Creating templates caches in build folder');

    return buildTemplateCache()
        .pipe(gulp.dest(config.build.temp));
});

gulp.task('_build-specs-templatecache', function() {
    log('Creating templates caches in build folder');

    return buildTemplateCache()
        .pipe(gulp.dest(config.build.testInput));
});

/**
 * Optimizing javascript, css and html files and saving these in build folder
 * @return {Stream}
 */
gulp.task('_optimize', ['bundle', '_build-templatecache'], function() {
    log('Optimizing javascript, css and html files and saving these in build folder');

    // TODO add angular and configure template cache for html files
    var templateCache = config.build.temp + config.build.templateCache.file;

    return gulp
        .src(config.index)
        .pipe(inject(templateCache, 'templates'))
        .pipe($.useref())
        .pipe(gulp.dest(config.build.output));
});

/**
 * Compiling Sass --> CSS
 * @return {Stream}
 */
gulp.task('sass', ['sass-lint'], function() {
    return gulp
        .src(config.styles.sass)
        .pipe($.sass({
            includePaths: config.sassPaths
        }).on('error', $.sass.logError))
        .pipe($.autoprefixer({
            browsers: [
                'Android 2.3',
                'Android >= 4',
                'Chrome >= 20',
                'Firefox >= 24',
                'Explorer >= 8',
                'iOS >= 6',
                'Opera >= 12',
                'Safari >= 6']
        }))
        .pipe(gulp.dest(config.styles.output));
});

/**
 * Lint all sass files
 * @return {Stream}
 */
gulp.task('sass-lint', function() {
    return gulp
        .src(config.styles.sass)
        .pipe($.cached('sass'))
        .pipe($.scssLint({
            'config': 'sass-lint.yml',
        }))
});

function buildTemplateCache() {
    return gulp
        .src(config.build.htmlTemplatesSrc)
        .pipe($.minifyHtml({ empty: true }))
        .pipe($.angularTemplatecache(
            config.build.templateCache.file,
            config.build.templateCache.options
        ));
}


/**
 * Inject files in a sorted sequence at a specified inject label
 * @param   {Array} src   glob pattern for source files
 * @param   {String} label   The label name
 * @param   {Array} order   glob pattern for sort order of the files
 * @returns {Stream}   The stream
 */
function inject(src, label, order, addPrefix) {
    var options = { read: false, addRootSlash: false };
    if (label) {
        options.name = 'inject:' + label;
    }
    if (addPrefix) {
        options.addPrefix = addPrefix;
    }

    return $.inject(orderSrc(src, order), options);
}

/**
 * Order a stream
 * @param   {Stream} src   The gulp.src stream
 * @param   {Array} order Glob array pattern
 * @returns {Stream} The ordered stream
 */
function orderSrc(src, order) {
    //order = order || ['**/*'];
    return gulp
        .src(src)
        .pipe($.if(order, $.order(order)));
}

/**
 * Log a message or series of messages using chalk's blue color.
 * Can pass in a string, object or array.
 */
function log(msg) {
    if (typeof (msg) === 'object') {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                $.util.log($.util.colors.blue(msg[item]));
            }
        }
    } else {
        $.util.log($.util.colors.blue(msg));
    }
}
