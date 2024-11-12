import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import http from 'k6/http';

// export const options = {
//     discardResponseBodies: true,
//     scenarios: {
//         contacts: {
//             executor: 'constant-vus',
//             vus: 10,
//             duration: '30s',
//         },
//     },
//     thresholds: {
//         http_req_duration: ['p(95)<300'], // 95% of requests must complete within 300ms
//     }
// };

const BASE_URL = 'http://fp_l:8080/products';
const NUM_PRODUCTS = 5;

const data = new SharedArray('productIds', () => Array.from({ length: NUM_PRODUCTS }, (_, i) => i + 1));

export default function () {
    const randomId = data[Math.floor(Math.random() * data.length)];

    // 1. Uncached request
    const uncachedResponse = http.get(`${BASE_URL}/cache/${randomId}`);
    check(uncachedResponse, {
        'status was 200': (r) => r.status === 200,
    });
    console.log(`Uncached request to product ${randomId} took ${uncachedResponse.timings.duration}ms`);

    // 2. Cached request
    const cachedResponse = http.get(`${BASE_URL}/cache/${randomId}`);
    check(cachedResponse, {
        'status was 200': (r) => r.status === 200,
    });
    console.log(`Cached request to product ${randomId} took ${cachedResponse.timings.duration}ms`);

    sleep(1);
}
