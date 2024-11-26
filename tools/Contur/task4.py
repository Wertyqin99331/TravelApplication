from bisect import bisect_right

N, M = map(int, input().split())
U, V = map(int, input().split())
u_lines = sorted(map(int, input().split()))
v_lines = sorted(map(int, input().split()))

q = int(input())
results = []

def find_region(x, y):
    x_region = bisect_right(u_lines, x)
    y_region = bisect_right(v_lines, y)
    return x_region, y_region

for _ in range(q):
    x1, y1, x2, y2 = map(int, input().split())

    region1 = find_region(x1, y1)
    region2 = find_region(x2, y2)

    results.append("YES" if region1 == region2 else "NO")

print("\n".join(results))
