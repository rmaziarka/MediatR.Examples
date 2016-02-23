module.exports = function () {
    var root = './';
    var clientApp = root + 'app/';

    var config = {
        ts: {
            allTs: clientApp + '**/*.ts',
            output: clientApp,
            outputAllJs: clientApp + '**/*.js',
            outputAllJsMap: clientApp + '**/*.js.map',
            libTypingsAllTs: root + 'libs/typings/**/*.ts'
        }
    };

    return config;
}