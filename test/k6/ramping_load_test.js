import { check, sleep } from 'k6';
import http from 'k6/http';

export const options = {
    discardResponseBodies: true,
    scenarios: {
        contacts: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                { duration: '20s', target: 10 },
                { duration: '10s', target: 0 },
            ],
            gracefulRampDown: '0s',
        },
    },
    thresholds: {
        http_req_duration: ['p(95)<300'], // 95% of requests must complete within 300ms
    },
    cloud: {
        projectID: 3721453,
        name: 'Test FP_L ramping-vus'
    }
};

export default function () {

    const randomId = getRandomInt(5) + 1;
    const response = http.get(`http://fp_l:8080/products/${randomId}`);

    check(response, {
        'is status 200': (x) => x.status === 200
    });

    sleep(Math.random() * 5);
}

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}
