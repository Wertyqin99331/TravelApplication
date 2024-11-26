N, k, q = map(int, input().split())
heights = list(map(int, input().split()))

max_length = 0
left = 0
change_count = 0

for right in range(N):
    if heights[right] >= k:
        change_count += 1

    while change_count > q:
        if heights[left] >= k:
            change_count -= 1
        left += 1

    max_length = max(max_length, right - left + 1)

print(max_length)
