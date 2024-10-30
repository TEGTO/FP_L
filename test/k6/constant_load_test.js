import { check, sleep } from 'k6';
import http from 'k6/http';

export const options = {
    discardResponseBodies: true,
    scenarios: {
        contacts: {
            executor: 'constant-vus',
            vus: 10,
            duration: '30s',
        },
    },
    thresholds: {
        http_req_duration: ['p(95)<300'], // 95% of requests must complete within 300ms
    },
    cloud: {
        projectID: 3721453,
        name: 'Test FP_L constant-vus'
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
